using FluentResults;
using MediatR;
using System.Transactions;
using WebStore.Domain;
using WebStore.EventBus;
using WebStore.Infrastructure.RabbitMq.Events;

namespace WebStore.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientRequestHandler(IClientRepository clientRepository, IOrderRepository orderRepository, IEventBus eventBus) :
        IRequestHandler<PutClientHandlingRequest, Result<Client>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<Result<Client>> Handle(PutClientHandlingRequest request, CancellationToken cancellationToken)
        {

            var client = (await Client.CreateClientAsync(request.Dto, _orderRepository)).Value;
            var integrationEvent = ClientEvent.CreateIntegrationEvent<ClientUpdatedEvent>(client);

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    Task.WaitAll([_clientRepository.UpdateClientAsync(client), _eventBus.PublishAsync(integrationEvent)], cancellationToken);
                    transaction.Complete();
                }

                return Result.Ok(client);
            }
            catch (Exception exception)
            {
                return Result.Fail(exception.Message);
            }
        }
    }
}
