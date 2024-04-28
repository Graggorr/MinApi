using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using WebStore.EventBus;

namespace WebStore.Infrastructure.RabbitMq
{
    public class EventBusRabbitMq : IEventBus
    {
        private const string EXCHANGE_NAME = "webstore_event_bus";

        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public EventBusRabbitMq(IOptions<RabbitMqConfiguration> options)
        {
            _connection = new ConnectionFactory() { HostName = "localhost" }.CreateConnection();
            _channel = _connection.CreateModel();
            _configuration = options.Value;
        }

        public void Publish<T>(T integrationEvent) where T : IntegrationEvent
        {
            var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent);
            _channel.BasicPublish(EXCHANGE_NAME, integrationEvent.RouteKey, null, body);
        }

        public async Task PublishAsync<T>(T integrationEvent) where T : IntegrationEvent => await Task.Factory.StartNew(() => Publish(integrationEvent));
    }
}
