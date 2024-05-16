namespace WebStore.EventBus.BackgroundJobService
{
    public interface IBackgroundJobProcesser
    {
        public Task ProcessJob();
    }
}
