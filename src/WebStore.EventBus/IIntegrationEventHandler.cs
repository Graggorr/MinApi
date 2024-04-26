namespace WebStore.EventBus;

public interface IIntegrationEventHandler<T> where T : IntegrationEvent
{
    public Task Handle(T integrationEvent);
}
