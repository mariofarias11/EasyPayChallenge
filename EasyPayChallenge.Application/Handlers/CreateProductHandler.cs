using System.Text.Json;
using EasyPayChallenge.Application.Repositories;
using EasyPayChallenge.Domain;
using EasyPayChallenge.Domain.Models.Commands;
using EasyPayChallenge.Domain.Models.Dto;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace EasyPayChallenge.Application.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IDatabase _redisDatabase;
    private readonly ILogger<CreateProductHandler> _logger;

    public CreateProductHandler(IProductRepository productRepository, 
        IConnectionMultiplexer connectionMultiplexer, 
        ILogger<CreateProductHandler> logger)
    {
        _productRepository = productRepository;
        _redisDatabase = connectionMultiplexer.GetDatabase();
        _logger = logger;
    }

    public async Task<ProductDto> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var currencyIsValid = Enum.TryParse<Currency>(command.Currency, out Currency currencyEnum);
            if (!currencyIsValid)
            {
                throw new ArgumentException($"Invalid currency: {command.Currency}");
            }
            
            var product = new Product
            {
                Name = command.Name,
                Brand = command.Brand,
                Price = new Money(command.PriceAmount, currencyEnum)
            };

            var productId = await _productRepository.Create(product);
            product.Id = productId;

            var productJson = JsonSerializer.Serialize(product, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                IgnoreNullValues = true
            });

            await _redisDatabase.StringSetAsync(productId.ToString(), productJson);

            _logger.LogInformation("Product created successfully. ID: {ProductId}", productId);

            return ProductDto.ConvertFromProduct(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating a product.");
            throw;
        }
    }
}