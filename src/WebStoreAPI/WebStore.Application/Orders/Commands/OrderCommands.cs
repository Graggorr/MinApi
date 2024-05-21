using FluentResults;
using MediatR;
using WebStore.API.Domain;

namespace WebStore.API.Application.Orders.Commands
{
    public record RegisterOrderRequest(Guid ClientId, string Name, double Price, string Description) : IRequest<Result<int>>;
    public record UpdateOrderRequest(Guid ClientId, int OrderId, string Name, double Price, string Description) : IRequest<Result<Order>>;
    public record DeleteOrderRequest(int Id) : IRequest<Result<Order>>;
}
