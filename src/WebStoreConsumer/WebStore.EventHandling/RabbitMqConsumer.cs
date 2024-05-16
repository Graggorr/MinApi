using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using WebStore.EventBus.Abstraction;

namespace WebStore.Consumer.RabbitMq
{
    public class RabbitMqConsumer<T> : IConsumer where T : IntegrationEvent
    {
        private const string EXCHANGE_NAME = "webstore_event_bus";

        private readonly IIntegrationEventHandler<T> _eventHandler;
        private readonly ILogger _logger;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly RabbitMqConfiguration _configuration;
        private readonly Type _type;

        public RabbitMqConsumer(IIntegrationEventHandler<T> eventHandler,
            IOptions<RabbitMqConfiguration> configuration, ILogger<IIntegrationEventHandler<T>> logger)
        {
            _eventHandler = eventHandler;
            _configuration = configuration.Value;
            _logger = logger;
            _type = typeof(T);
            _connection = new ConnectionFactory { HostName = _configuration.HostName }.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Result Consume()
        {
            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received += OnReceived;

            var typeName = _type.Name;
            var index = typeName.IndexOf("Client");
            var queueName = $"client_{typeName.Remove(index).ToLower()}";

            _channel.BasicQos(0, 1, false);
            PrepareQueue(_channel, queueName);
            _channel.BasicConsume(queueName, false, consumer);

            return Result.Ok();
        }

        private async Task OnReceived(object sender, BasicDeliverEventArgs args)
        {
            var message = Encoding.UTF8.GetString(args.Body.Span);
            var integrationEvent = JsonSerializer.Deserialize<T>(message);
            _logger.LogInformation($"Got a new integration event of type {_type}. ID: {integrationEvent.Id}");
            var result = await _eventHandler.Handle(integrationEvent);

            if (result.IsSuccess)
            {
                _channel.BasicAck(args.DeliveryTag, false);
            }
        }

        private static void PrepareQueue(IModel channel, string queueName)
        {
            channel.ExchangeDeclare(EXCHANGE_NAME, "direct", true, false, null);
            channel.QueueDeclare(queueName, true, false, false, null);
            channel.QueueBind(queueName, EXCHANGE_NAME, "users/players/customers");
        }
    }
}
