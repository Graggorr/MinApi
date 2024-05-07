using FluentResults;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using WebStore.EventBus;

namespace WebStore.RabbitMqEventHandling
{
    public class RabbitMqConsumer<T>(IIntegrationEventHandler<T> eventHandler,
        IOptions<RabbitMqConfiguration> configuration) : IConsumer where T : IntegrationEvent
    {
        private readonly IIntegrationEventHandler<T> _eventHandler = eventHandler;
        private readonly RabbitMqConfiguration _configuration = configuration.Value;

        public Result Consume()
        {
            var connection = new ConnectionFactory { HostName = _configuration.HostName }.CreateConnection();
            var channel = connection.CreateModel();

            var consumer = new AsyncEventingBasicConsumer(channel);

            consumer.Received += async (sender, args) =>
            {
                var message = Encoding.UTF8.GetString(args.Body.Span);
                var integrationEvent = JsonSerializer.Deserialize<T>(message);
                var result = await _eventHandler.Handle(integrationEvent);

                if (result.IsSuccess)
                {
                    channel.BasicAck(args.DeliveryTag, false);
                }
            };

            var typeName = typeof(T).Name;
            var index = typeName.IndexOf("Client");
            var queueName = $"client_{typeName.Remove(index)}";

            channel.BasicConsume(queueName, false, consumer);

            return Result.Ok();
        }
    }
}
