using FluentResults;

namespace WebStore.EventBus;

public interface IEventBus
{
    public void Publish<T>(T integrationEvent) where T : IntegrationEvent;
    public Task PublishAsync<T>(T integrationEvent) where T : IntegrationEvent;
}
