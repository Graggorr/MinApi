using WebStore.EventBus.Events;

namespace WebStore.EventBus.Common;

public interface IEventBus
{
    public void Publish(IntegrationEvent integrationEvent);
}
