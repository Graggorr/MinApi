using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using WebStore.API.Infrastructure.Clients;
using WebStore.API.Infrastructure.Orders;
using WebStore.API.Domain;
using WebStore.Extensions;

namespace WebStore.API.Application.Orders.Commands.CreateOrder
{
    public class UpdateOrderRequestHandler(ILogger<IRequestHandler<UpdateOrderRequest, Result<Order>>> logger,
       IClientRepository clientRepository, IOrderRepository orderRepository) : IRequestHandler<UpdateOrderRequest, Result<Order>>
    {
        private readonly ILogger _logger = logger;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task<Result<Order>> Handle(UpdateOrderRequest request, CancellationToken cancellationToken)
        {
            var clientResult = await _clientRepository.GetClientAsync(request.ClientId);

            if (clientResult.IsFailed)
            {
                return _logger.LogSendAndFail($"Client with ID: {request.ClientId} is not found to update an order");
            }

            var client = clientResult.Value;

            var order = new Order()
            {
                Id = request.OrderId,
                ClientId = client.Id,
                Client = client,
                Description = request.Description,
                Name = request.Name,
                Price = request.Price,
            };

            var result = await _orderRepository.UpdateOrderAsync(order);

            if (result.IsSuccess)
            {
                return Result.Ok(order);
            }

            return Result.Fail(result.Errors);
        }
    }
}
