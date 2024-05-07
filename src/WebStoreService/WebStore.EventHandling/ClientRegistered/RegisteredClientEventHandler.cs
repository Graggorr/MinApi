﻿using FluentResults;
using System.Net.Mail;
using System.Text;
using WebStore.EventBus;

namespace WebStore.RabbitMqEventHandling.ClientRegistered
{
    public class RegisteredClientEventHandler : IIntegrationEventHandler<CreatedClientEvent>
    {
        public async Task<Result> Handle(CreatedClientEvent integrationEvent)
        {
            var emailSender = "graggorr@gmail.com";
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"Hi, {integrationEvent.Name}.\nYou've been successfully processed. Your orders have been add to execution.");
            stringBuilder.AppendLine($"Your orders: {integrationEvent.Orders}");

            var mail = new MailMessage(emailSender, integrationEvent.Email, "WebStore", stringBuilder.ToString());
            var client = new SmtpClient("localhost");

            try
            {
                client.Send(mail);

                return Result.Ok();
            }
            catch (Exception exception)
            {
                return Result.Fail(exception.Message);
            }
        }
    }
}