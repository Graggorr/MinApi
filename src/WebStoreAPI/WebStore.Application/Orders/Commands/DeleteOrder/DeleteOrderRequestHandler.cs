using FluentResults;
using MediatR;
using WebStore.API.Infrastructure.Orders;
using WebStore.API.Domain;

namespace WebStore.API.Application.Orders.Commands.CreateOrder
{
    public class DeleteOrderRequestHandler(IOrderRepository orderRepository) : IRequestHandler<DeleteOrderRequest, Result<Order>>
    {
        private readonly IOrderRepository _orderRepository = orderRepository;

        public async Task<Result<Order>> Handle(DeleteOrderRequest request, CancellationToken cancellationToken) 
            => await _orderRepository.DeleteOrderAsync(request.Id);
    }
}
