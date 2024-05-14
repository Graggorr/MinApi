using FluentResults;

namespace WebStore.EventBus.Abstraction;

public interface IEventBus
{
    public Result Publish<T>(T integrationEvent) where T : IntegrationEvent;
    public Task<Result> PublishAsync<T>(T integrationEvent) where T : IntegrationEvent;
}
