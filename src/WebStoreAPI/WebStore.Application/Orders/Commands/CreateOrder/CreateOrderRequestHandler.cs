using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using WebStore.API.Infrastructure.Clients;
using WebStore.API.Infrastructure.Orders;
using WebStore.API.Domain;
using WebStore.Extensions;

namespace WebStore.API.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderRequestHandler(ILogger<IRequestHandler<RegisterOrderRequest, Result<int>>> logger,
       IClientRepository clientRepository, IOrderRepository orderRepository) : IRequestHandler<RegisterOrderRequest, Result<int>>
    {
        private readonly ILogger _logger = logger;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task<Result<int>> Handle(RegisterOrderRequest request, CancellationToken cancellationToken)
        {
            var clientResult = await _clientRepository.GetClientAsync(request.ClientId);

            if (clientResult.IsFailed)
            {
                return _logger.LogSendAndFail($"Client with ID: {request.ClientId} is not found to attach an order to them");
            }

            var client = clientResult.Value;

            var order = new Order()
            {
                ClientId = client.Id,
                Client = client,
                Description = request.Description,
                Name = request.Name,
                Price = request.Price,
            };

            var orderId = await _orderRepository.AddOrderAsync(order);

            return Result.Ok(orderId);
        }
    }
}
