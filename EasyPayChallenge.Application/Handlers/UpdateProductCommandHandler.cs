using EasyPayChallenge.Application.Repositories;
using EasyPayChallenge.Domain.Models.Commands;
using EasyPayChallenge.Domain.Models.Dto;
using EasyPayChallenge.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IDatabase _redisDatabase;
    private readonly ILogger<UpdateProductCommandHandler> _logger;

    public UpdateProductCommandHandler(IProductRepository productRepository,
                                       IConnectionMultiplexer connectionMultiplexer,
                                       ILogger<UpdateProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _redisDatabase = connectionMultiplexer.GetDatabase();
        _logger = logger;
    }

    public async Task<ProductDto> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
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
                Id = command.Id,
                Name = command.Name,
                Brand = command.Brand,
                Price = new Money(command.PriceAmount, currencyEnum)
            };

            var productJson = JsonSerializer.Serialize(product);

            var updated = await _productRepository.Update(product);

            if (!updated)
            {
                _logger.LogWarning("Product update failed. ID: {ProductId}", product.Id);
                return null;
            }

            await _redisDatabase.StringSetAsync(product.Id.ToString(), productJson);

            _logger.LogInformation("Product updated successfully. ID: {ProductId}", product.Id);
            return ProductDto.ConvertFromProduct(product);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating a product. ID: {ProductId}", command.Id);
            throw; 
        }
    }
}
