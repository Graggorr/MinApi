using FluentResults;

namespace WebStore.EventBus.RabbitMq
{
    public interface IEventProcesser
    {
        public Task<Result> ProceedEventsAsync();
    }
}
