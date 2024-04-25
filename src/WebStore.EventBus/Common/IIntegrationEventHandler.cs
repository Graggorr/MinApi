using WebStore.EventBus.Events;

namespace WebStore.EventBus.Common;

public interface IIntegrationEventHandler<T> where T : IntegrationEvent
{
    public Task Handle(T @event);
}
