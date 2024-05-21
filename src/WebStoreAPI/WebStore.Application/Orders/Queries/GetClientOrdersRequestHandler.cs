using FluentResults;
using MediatR;
using WebStore.API.Application.Orders.Queries;
using WebStore.API.Domain;
using WebStore.API.Infrastructure.Orders;

namespace WebStore.API.Application.Clients.Queries
{
    public class GetClientOrdersRequestHandler(IOrderRepository repository) : IRequestHandler<GetClientOrdersRequest, Result<IEnumerable<Order>>>
    {
        private readonly IOrderRepository _repository = repository;

        public async Task<Result<IEnumerable<Order>>> Handle(GetClientOrdersRequest request, CancellationToken cancellationToken)
            => await _repository.GetClientOrdersAsync(request.ClientId);
    }
}
