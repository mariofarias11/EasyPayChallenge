using EasyPayChallenge.Application.Repositories;
using EasyPayChallenge.Domain.Models.Dto;
using EasyPayChallenge.Domain.Models.Queries;
using EasyPayChallenge.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Text.Json;

public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IDatabase _redisDatabase;
    private readonly ILogger<GetProductByIdQueryHandler> _logger;

    public GetProductByIdQueryHandler(IProductRepository productRepository,
                                      IConnectionMultiplexer connectionMultiplexer,
                                      ILogger<GetProductByIdQueryHandler> logger)
    {
        _productRepository = productRepository;
        _redisDatabase = connectionMultiplexer.GetDatabase();
        _logger = logger;
    }

    public async Task<ProductDto> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var product = await GetProductFromCache(query.Id);
            if (product != null)
            {
                return ProductDto.ConvertFromProduct(product);
            }

            var result = await _productRepository.GetById(query.Id);
            if (result == null)
            {
                _logger.LogInformation("Product not found. ID: {ProductId}", query.Id);
                return null;
            }

            var productJson = JsonSerializer.Serialize(result);

            await _redisDatabase.StringSetAsync(query.Id.ToString(), productJson);

            _logger.LogInformation("Product retrieved from database and cached. ID: {ProductId}", query.Id);
            return ProductDto.ConvertFromProduct(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving a product by ID: {ProductId}", query.Id);
            throw;
        }
    }

    private async Task<Product?> GetProductFromCache(int id)
    {
        var value = await _redisDatabase.StringGetAsync(id.ToString());
        return value.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Product>(value);
    }
}