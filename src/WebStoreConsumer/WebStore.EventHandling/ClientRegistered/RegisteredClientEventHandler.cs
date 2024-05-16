using FluentResults;
using Microsoft.Extensions.Logging;
using WebStore.EventBus.Abstraction;

namespace WebStore.Consumer.RabbitMq.ClientRegistered
{
    public class RegisteredClientEventHandler(ILogger<IIntegrationEventHandler<CreatedClientEvent>> logger) : IIntegrationEventHandler<CreatedClientEvent>
    {
        public async Task<Result> Handle(CreatedClientEvent integrationEvent)
        {
            logger.LogInformation($"{integrationEvent.Id} is processed");
            logger.LogInformation("Slavik");

            return Result.Ok();
            //var emailSender = "graggorr@gmail.com";
            //var stringBuilder = new StringBuilder();

            //stringBuilder.Append($"Hi, {integrationEvent.Name}.\nYou've been successfully registered. Your orders have been add to execution.");
            //stringBuilder.AppendLine($"Your orders: {integrationEvent.Orders}");

            //var mail = new MailMessage(emailSender, integrationEvent.Email, "WebStore", stringBuilder.ToString());
            //var client = new SmtpClient("localhost");

            //try
            //{
            //    client.Send(mail);

            //    return Result.Ok();
            //}
            //catch (Exception exception)
            //{
            //    return Result.Fail(exception.Message);
            //}
        }
    }
}
