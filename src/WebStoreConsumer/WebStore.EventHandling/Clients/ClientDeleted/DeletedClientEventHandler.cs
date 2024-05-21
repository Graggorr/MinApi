using FluentResults;
using Microsoft.Extensions.Logging;
using WebStore.EventBus.Abstraction;

namespace WebStore.Consumer.RabbitMq.Clients.ClientDeleted
{
    public class DeletedClientEventHandler(ILogger<IIntegrationEventHandler<DeletedClientEvent>> logger)
        : ClientEventHandler<DeletedClientEvent>(logger)
    {
        public override Task<Result> Handle(DeletedClientEvent integrationEvent)
            => HandleInternal(integrationEvent, "Your account has been deleted successfully");
    }
}
