namespace WebStore.EventBus;

public interface IEventBus
{
    public void Publish<T>(T integrationEvent) where T : IntegrationEvent;
}
