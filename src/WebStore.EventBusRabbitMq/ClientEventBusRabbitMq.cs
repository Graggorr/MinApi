using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using WebStore.EventBus.Common;
using WebStore.EventBus.Events;

namespace WebStore.EventBusRabbitMq
{
    public class ClientEventBusRabbitMq(RabbitMqConfiguration configuration) : IEventBus, IDisposable, IHostedService
    {
        private const string EXCHANGE_NAME = "webstore_event_bus";

        private IConnection? _subscriptionConnection;
        private IModel? _channel;
        private RabbitMqConfiguration _configuration = configuration;
        private bool _disposed;

        public void Publish(IntegrationEvent integrationEvent)
        {
            using var connection = CreateConnection(_configuration);
            using var channel = connection.CreateModel();
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(integrationEvent));

            try
            {
                channel.BasicPublish(EXCHANGE_NAME, integrationEvent.Postfix, null, body);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await Task.Factory.StartNew(() =>
            {
                _subscriptionConnection = CreateConnection(_configuration);
                _channel = _subscriptionConnection.CreateModel();
                var consumer = new AsyncEventingBasicConsumer(_channel);

                _channel.BasicConsume(_configuration.QueueName, false, consumer);

            }, cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _channel?.Close();
                    _channel?.Dispose();
                    _channel = null;
                    _subscriptionConnection?.Close();
                    _subscriptionConnection?.Dispose();
                    _subscriptionConnection = null;
                    _configuration = null;
                }

                _disposed = true;
            }
        }

        private static IConnection CreateConnection(RabbitMqConfiguration configuration) => new ConnectionFactory() { HostName = configuration.HostName, Port = configuration.Port }.CreateConnection();
    }
}
