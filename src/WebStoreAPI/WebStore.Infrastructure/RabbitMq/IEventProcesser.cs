using WebStore.EventBus;

namespace WebStore.Infrastructure
{
    public interface IEventProcesser
    {
        public Task CallProcessing();
    }
}
