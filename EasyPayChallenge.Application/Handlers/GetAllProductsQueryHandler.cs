using EasyPayChallenge.Application.Repositories;
using EasyPayChallenge.Domain.Models.Dto;
using EasyPayChallenge.Domain.Models.Queries;
using MediatR;
using Microsoft.Extensions.Logging;

public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly ILogger<GetAllProductsQueryHandler> _logger;

    public GetAllProductsQueryHandler(IProductRepository productRepository, ILogger<GetAllProductsQueryHandler> logger)
    {
        _productRepository = productRepository;
        _logger = logger;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetAllProductsQuery query, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productRepository.GetAll();
            return result.Select(x => ProductDto.ConvertFromProduct(x));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving all products.");
            throw; 
        }
    }
}
