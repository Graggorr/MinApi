using FluentResults;
using WebStore.EventBus;

namespace WebStore.EventHandling.ClientRegistered
{
    public class RegisteredClientEventHandler : IIntegrationEventHandler<CreatedClientEvent>
    {
        public async Task<Result> Handle(CreatedClientEvent integrationEvent)
        {
            throw new NotImplementedException();
        }
    }
}
