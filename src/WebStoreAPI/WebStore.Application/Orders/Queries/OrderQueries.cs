using FluentResults;
using MediatR;
using WebStore.API.Domain;

namespace WebStore.API.Application.Orders.Queries
{
    public record GetOrderRequest(int Id) : IRequest<Result<Order>>;
    public record GetClientOrdersRequest(Guid ClientId): IRequest<Result<IEnumerable<Order>>>;
}
