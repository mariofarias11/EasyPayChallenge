using EasyPayChallenge.Application.Repositories;
using EasyPayChallenge.Domain.Models.Commands;
using MediatR;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;
    private readonly IDatabase _redisDatabase;
    private readonly ILogger<DeleteProductCommandHandler> _logger;

    public DeleteProductCommandHandler(IProductRepository productRepository,
                                       IConnectionMultiplexer connectionMultiplexer,
                                       ILogger<DeleteProductCommandHandler> logger)
    {
        _productRepository = productRepository;
        _redisDatabase = connectionMultiplexer.GetDatabase();
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var deleted = await _productRepository.Delete(command.Id);

            if (deleted)
            {
                await _redisDatabase.KeyDeleteAsync(command.Id.ToString());
                _logger.LogInformation("Product deleted successfully. ID: {ProductId}", command.Id);
            }
            else
            {
                _logger.LogWarning("Product deletion failed. ID: {ProductId}", command.Id);
            }

            return deleted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting a product. ID: {ProductId}", command.Id);
            throw;
        }
    }
}
