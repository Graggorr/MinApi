using FluentResults;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using WebStore.EventBus.Abstraction;

namespace WebStore.EventBus.RabbitMq
{
    public class EventBusRabbitMq : IEventBus
    {
        private const string EXCHANGE_NAME = "webstore_event_bus";

        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public EventBusRabbitMq(IOptions<RabbitMqConfiguration> options)
        {
            _configuration = options.Value;
            _connection = new ConnectionFactory() { HostName = _configuration.HostName }.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Result Publish<T>(T integrationEvent) where T : IntegrationEvent
        {
            try
            {
                var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent);
                PrepareQueue(_channel, integrationEvent.QueueName);
                _channel.BasicPublish(EXCHANGE_NAME, integrationEvent.RouteKey, null, body);

                return Result.Ok();
            }
            catch (Exception exception)
            {
                return Result.Fail(exception.Message);
            }
        }

        public async Task<Result> PublishAsync<T>(T integrationEvent) where T : IntegrationEvent
            => await Task.Factory.StartNew(() => Publish(integrationEvent));

        private static void PrepareQueue(IModel channel, string queueName)
        {
            channel.ExchangeDeclare(EXCHANGE_NAME, "direct", true, false, null);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, EXCHANGE_NAME, "users/players/customers");
        }
    }
}
