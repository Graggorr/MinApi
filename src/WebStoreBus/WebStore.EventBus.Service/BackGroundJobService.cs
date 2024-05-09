using Hangfire;
using WebStore.EventBus.RabbitMq;

namespace WebStore.EventBus.Service
{
    public class BackGroundJobService(IEventProcesser eventProcesser) : BackgroundService
    {
        private readonly IEventProcesser _eventProcesser = eventProcesser;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            RecurringJob.AddOrUpdate(nameof(_eventProcesser.ProceedEventsAsync),
                () => _eventProcesser.ProceedEventsAsync(), "5 * * * * *");
        }
    }
}
