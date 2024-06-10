using MediatR;

namespace EasyPayChallenge.Domain.Models.Commands;

public class DeleteProductCommand : IRequest<bool>
{
    public int Id { get; set; }
}