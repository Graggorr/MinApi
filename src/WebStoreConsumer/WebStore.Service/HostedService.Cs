using WebStore.EventBus.Abstraction;

namespace WebStore.Service
{
    public class HostedService(IEnumerable<IConsumer> consumers) : IHostedService
    {
        private readonly IEnumerable<IConsumer> _consumers = consumers;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                foreach (var consumer in _consumers)
                {
                    consumer.Consume();
                }
            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
