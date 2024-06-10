using EasyPayChallenge.Domain.Models.Dto;
using MediatR;

namespace EasyPayChallenge.Domain.Models.Queries;

public class GetAllProductsQuery :  IRequest<IEnumerable<ProductDto>>
{
    
}