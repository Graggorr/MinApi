using FluentResults;
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

        public Result Publish<T>(T integrationEvent) where T : IntegrationEvent
        {
            try
            {
                //using var connection = new ConnectionFactory()
                //{
                //    UserName = _configuration.UserName,
                //    Password = _configuration.Password,
                //    HostName = _configuration.HostName,
                //    Port = _configuration.Port,
                //    VirtualHost = _configuration.VirtualHost,
                //    AutomaticRecoveryEnabled = _configuration.AutomaticRecoveryEnabled,
                //    Ssl = _configuration.SslOption
                //}.CreateConnection();
                var factory = new ConnectionFactory { HostName = "localhost" };
                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                var body = JsonSerializer.SerializeToUtf8Bytes(integrationEvent);
                channel.BasicPublish(EXCHANGE_NAME, integrationEvent.RouteKey, null, body);

                return Result.Ok();
            }
            catch (Exception exception)
            {
                return Result.Fail(exception.Message);
            }
        }
    }
}
