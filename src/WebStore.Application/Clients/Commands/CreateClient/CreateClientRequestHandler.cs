using FluentResults;
using MediatR;
using System.Transactions;
using WebStore.Domain;
using WebStore.EventBus;
using WebStore.Infrastructure.RabbitMq.Events;

namespace WebStore.Application.Clients.Commands.CreateClient
{
    public class CreateClientRequestHandler(IClientRepository clientRepository, IOrderRepository orderRepository, IEventBus eventBus)
        : IRequestHandler<PostClientHandlingRequest, Result<Client>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<Result<Client>> Handle(PostClientHandlingRequest request, CancellationToken cancellationToken)
        {
            var client = (await Client.CreateClientAsync(request.Dto, _orderRepository)).Value;
            var integrationEvent = ClientEvent.CreateIntegrationEvent<ClientCreatedEvent>(client);

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    Task.WaitAll([_clientRepository.AddClientAsync(client), _eventBus.PublishAsync(integrationEvent)], cancellationToken);
                    transaction.Complete();
                }

                return Result.Ok(client);
            }
            catch (Exception exception)
            {
                return Result.Fail($"Cannot handle {client.ToStringWithoutId()}. Exception: {exception.Message}");
            }
        }
    }
}
