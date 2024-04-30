using FluentResults;
using MediatR;
using System.Transactions;
using WebStore.Domain;
using WebStore.EventBus;
using WebStore.Infrastructure.RabbitMq.Events;

namespace WebStore.Application.Clients.Commands.DeleteClient
{
    public class DeleteClientRequestHandler(IClientRepository repository, IEventBus eventBus) : IRequestHandler<DeleteClientHandlingRequest, Result<Client>>
    {
        private readonly IClientRepository _repository = repository;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<Result<Client>> Handle(DeleteClientHandlingRequest request, CancellationToken cancellationToken)
        {
            try
            {
                using var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
                var client = (await _repository.DeleteClientAsync(request.Id)).Value;
                var integrationEvent = ClientEvent.CreateIntegrationEvent<ClientDeletedEvent>(client);
                _eventBus.Publish(integrationEvent);
                transaction.Complete();

                return Result.Ok(client);
            }
            catch (Exception exception)
            {
                return Result.Fail(exception.Message);
            }
        }
    }
}
