using FluentResults;
using MediatR;
using WebStore.API.Application.Orders.Queries;
using WebStore.API.Domain;
using WebStore.API.Infrastructure.Orders;

namespace WebStore.API.Application.Clients.Queries
{
    public class GetOrderRequestHandler(IOrderRepository repository) : IRequestHandler<GetOrderRequest, Result<Order>>
    {
        private readonly IOrderRepository _repository = repository;

        public async Task<Result<Order>> Handle(GetOrderRequest request, CancellationToken cancellationToken)
            => await _repository.GetOrderAsync(request.Id);
    }
}
