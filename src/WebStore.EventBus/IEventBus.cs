using FluentResults;

namespace WebStore.EventBus;

public interface IEventBus
{
    public Result Publish<T>(T integrationEvent) where T : IntegrationEvent;
}
