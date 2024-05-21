using FluentResults;
using Microsoft.Extensions.Logging;
using WebStore.EventBus.Abstraction;

namespace WebStore.Consumer.RabbitMq.Clients.ClientRegistered
{
    public class RegisteredClientEventHandler(ILogger<IIntegrationEventHandler<RegisteredClientEvent>> logger)
        : ClientEventHandler<RegisteredClientEvent>(logger)
    {
        public override Task<Result> Handle(RegisteredClientEvent integrationEvent)
            => HandleInternal(integrationEvent, "Your account has been deleted successfully");
    }
}
