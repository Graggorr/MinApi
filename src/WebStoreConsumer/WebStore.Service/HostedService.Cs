using WebStore.EventBus.Abstraction;

namespace WebStore.Service
{
    public class HostedService(IEnumerable<IConsumer> consumers) : BackgroundService
    {
        private readonly IEnumerable<IConsumer> _consumers = consumers;

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            foreach (var consumer in _consumers)
            {
                await Task.Factory.StartNew(consumer.Consume, stoppingToken);
            }
        }
    }
}
