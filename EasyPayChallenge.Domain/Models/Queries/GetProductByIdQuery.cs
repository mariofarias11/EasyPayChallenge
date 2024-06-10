using EasyPayChallenge.Domain.Models.Dto;
using MediatR;

namespace EasyPayChallenge.Domain.Models.Queries;

public class GetProductByIdQuery : IRequest<ProductDto>
{
    public int Id { get; set; }
}