using FluentResults;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Transactions;
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
            var integrationEvent = ClientEvent.CreateIntegrationEvent<ClientCreatedEvent>(client);

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    Task.WaitAll([_repository.AddClientAsync(client), _eventBus.PublishAsync(integrationEvent)], cancellationToken);
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
