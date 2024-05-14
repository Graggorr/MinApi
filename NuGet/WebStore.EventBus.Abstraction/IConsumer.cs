using FluentResults;

namespace WebStore.EventBus.Abstraction
{
    public interface IConsumer
    {
        public Result Consume();
    }
}
