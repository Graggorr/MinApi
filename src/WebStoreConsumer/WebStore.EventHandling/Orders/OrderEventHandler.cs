using FluentResults;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Net.Mail;
using WebStore.EventBus.Abstraction;
using WebStore.Events.Orders;
using WebStore.Extensions;
using Dapper;

namespace WebStore.Consumer.RabbitMq.Orders
{
    public abstract class OrderEventHandler<T>(IDbConnection connection, ILogger<IIntegrationEventHandler<T>> logger)
        : IIntegrationEventHandler<T> where T : OrderEvent
    {
        private readonly ILogger _logger = logger;
        private readonly IDbConnection _connection = connection;

        public abstract Task<Result> Handle(T integrationEvent);

        protected internal async Task<Result> HandleInternal(T integrationEvent, string message)
        {
            if (_connection.State is not ConnectionState.Open)
            {
                _connection.Open();
            }

            var sqlString = $"SELECT Email FROM webstore WHERE ClientId = @ClientId";

            var result =  await Task.Factory.StartNew(() =>
            {
                try
                {
                    var email = _connection.Query<string>(sqlString, integrationEvent).Single();
                    var mail = new MailMessage("graggorr@gmail.com", email, "WebStore", message);
                    var client = new SmtpClient("localhost");
                    client.Send(mail);
                    _logger.LogInformation($"{nameof(T)} with Id: {integrationEvent.Id} has been proceeded.");

                    return Result.Ok();
                }
                catch (Exception exception)
                {
                    return _logger.LogSendAndFail(exception.Message);
                }
            });

            _connection.Close();

            return result;
        }
    }
}
