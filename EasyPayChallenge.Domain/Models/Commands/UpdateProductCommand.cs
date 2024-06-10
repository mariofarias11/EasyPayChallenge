using EasyPayChallenge.Domain.Models.Dto;
using MediatR;

namespace EasyPayChallenge.Domain.Models.Commands;

public class UpdateProductCommand : IRequest<ProductDto>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Brand { get; set; }
    public decimal PriceAmount { get; set; }
    public string Currency { get; set; } = "USD";
}