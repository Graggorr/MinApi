using FluentResults;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using WebStore.EventBus.Abstraction;
using WebStore.Events.Clients;
using WebStore.Extensions;

namespace WebStore.Consumer.RabbitMq.Clients
{
    public abstract class ClientEventHandler<T>(ILogger<IIntegrationEventHandler<T>> logger)
        : IIntegrationEventHandler<T> where T : ClientEvent
    {
        private readonly ILogger _logger = logger;

        public abstract Task<Result> Handle(T integrationEvent);

        protected internal async Task<Result> HandleInternal(T integrationEvent, string message)
        {
            var mail = new MailMessage("graggorr@gmail.com", integrationEvent.Email, "WebStore", message);
            var client = new SmtpClient("localhost");

            return await Task.Factory.StartNew(() =>
            {
                try
                {
                    client.Send(mail);
                    _logger.LogInformation($"{nameof(T)} with Id: {integrationEvent.Id} has been proceeded.");

                    return Result.Ok();
                }
                catch (Exception exception)
                {
                    return _logger.LogSendAndFail(exception.Message);
                }
            });
        }
    }
}
