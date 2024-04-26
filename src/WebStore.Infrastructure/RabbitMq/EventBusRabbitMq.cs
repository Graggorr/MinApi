using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using WebStore.EventBus;

namespace WebStore.Infrastructure.RabbitMq
{
    public class EventBusRabbitMq(IOptions<RabbitMqConfiguration> options) : IEventBus
    {
        private const string EXCHANGE_NAME = "webstore_event_bus";

        private readonly RabbitMqConfiguration _configuration = options.Value;

        public void Publish<T>(T integrationEvent) where T : IntegrationEvent
        {
            using var connection = CreateConnection();
            using var channel = connection.CreateModel();
            var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent);

            try
            {
                channel.BasicPublish(EXCHANGE_NAME, integrationEvent.RouteKey, null, body);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        private IConnection CreateConnection() => new ConnectionFactory() { HostName = _configuration.HostName, Port = _configuration.Port }.CreateConnection();
    }
}
