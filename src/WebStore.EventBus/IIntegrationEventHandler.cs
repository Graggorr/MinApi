using FluentResults;

namespace WebStore.EventBus;

public interface IIntegrationEventHandler<T> where T : IntegrationEvent
{
    public Task<Result> Handle(T integrationEvent);
}
