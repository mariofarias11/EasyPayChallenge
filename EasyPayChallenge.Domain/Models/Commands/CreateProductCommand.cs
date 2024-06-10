using EasyPayChallenge.Domain.Models.Dto;
using MediatR;

namespace EasyPayChallenge.Domain.Models.Commands;

public class CreateProductCommand : IRequest<ProductDto>
{
    public string Name { get; set; }
    public string Brand { get; set; }
    public decimal PriceAmount { get; set; }
    public string Currency { get; set; } = "USD";
}