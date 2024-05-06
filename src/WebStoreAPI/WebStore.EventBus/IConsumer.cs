using FluentResults;

namespace WebStore.EventBus
{
    public interface IConsumer
    {
        public Result Consume();
    }
}
