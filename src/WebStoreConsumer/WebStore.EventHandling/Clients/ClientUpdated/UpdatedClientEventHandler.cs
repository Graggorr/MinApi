using FluentResults;
using Microsoft.Extensions.Logging;
using WebStore.EventBus.Abstraction;

namespace WebStore.Consumer.RabbitMq.Clients.ClientUpdated
{
    public class UpdatedClientEventHandler(ILogger<IIntegrationEventHandler<UpdatedClientEvent>> logger)
        : ClientEventHandler<UpdatedClientEvent>(logger)
    {
        public override Task<Result> Handle(UpdatedClientEvent integrationEvent)
            => HandleInternal(integrationEvent, "Your account has been updated successfully");
    }
}
