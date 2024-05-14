using WebStore.EventBus.Abstraction;

namespace WebStore.Service
{
    public class HostedService(IEnumerable<IConsumer> consumers) : IHostedService
    {
        private readonly IEnumerable<IConsumer> _consumers = consumers;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(() =>
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
