using FluentResults;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text.Json;
using WebStore.EventBus.Abstraction;
using WebStore.EventBus.Abstraction.Extensions;
using WebStore.Extensions;

namespace WebStore.EventBus.RabbitMq
{
    public class EventBusRabbitMq : IEventBus
    {
        private const string EXCHANGE_NAME = "webstore_event_bus";

        private readonly RabbitMqConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger _logger;

        public EventBusRabbitMq(IOptions<RabbitMqConfiguration> options, ILogger<IEventBus> logger)
        {
            _logger = logger;
            _configuration = options.Value;
            _connection = new ConnectionFactory() { HostName = _configuration.HostName }.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public Result Publish<T>(T integrationEvent) where T : IntegrationEvent
        {
            try
            {
                var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent);
                _channel.InitializeQueue(EXCHANGE_NAME, integrationEvent.QueueName, integrationEvent.RouteKey);
                _channel.BasicPublish(EXCHANGE_NAME, integrationEvent.RouteKey, null, body);
                _logger.LogInformation($"{typeof(T).Name} with Id: {integrationEvent.Id} has been published successfully.");

                return Result.Ok();
            }
            catch (Exception exception)
            {
                return _logger.LogSendAndFail(exception.Message);
            }
        }

        public async Task<Result> PublishAsync<T>(T integrationEvent) where T : IntegrationEvent
            => await Task.Factory.StartNew(() => Publish(integrationEvent));
    }
}
