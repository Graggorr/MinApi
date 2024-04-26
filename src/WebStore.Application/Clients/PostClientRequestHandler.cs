using FluentResults;
using MediatR;
using WebStore.Domain;
using WebStore.EventBus;
using WebStore.Infrastructure.RabbitMq.Events;

namespace WebStore.Application.Clients
{
    public class PostClientRequestHandler(IClientRepository clientRepository, IOrderRepository orderRepository, IEventBus eventBus)
        : IRequestHandler<PostClientHandlingRequest, Result<Client>>
    {
        private readonly IClientRepository _repository = clientRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<Result<Client>> Handle(PostClientHandlingRequest request, CancellationToken cancellationToken)
        {
            var clientResult = await Client.CreateClientAsync(request.Dto, _repository, _orderRepository, true, true);

            if (clientResult.IsFailed)
            {
                return Result.Fail(clientResult.Errors);
            }

            var client = clientResult.Value;

            var result = await _repository.AddClientAsync(client);

            if (result)
            {
                var integrationEvent = new ClientCreatedEvent(client.Id.ToString(), client.Name, client.PhoneNumber,
                    client.Email, client.Orders.Select(x => x.ToStringWithoutClients()).ToList());
                _eventBus.Publish(integrationEvent);

                return Result.Ok(client);
            }

            return Result.Fail($"Cannot handle {client.ToStringWithoutId()}");
        }
    }
}
