using FluentResults;
using Microsoft.Extensions.Logging;
using System.Data;
using WebStore.EventBus.Abstraction;

namespace WebStore.Consumer.RabbitMq.Orders.OrderRegistered
{
    public class RegisteredOrderEventHandler(IDbConnection connection, ILogger<IIntegrationEventHandler<RegisteredOrderEvent>> logger)
        : OrderEventHandler<RegisteredOrderEvent>(connection, logger)
    {
        public override Task<Result> Handle(RegisteredOrderEvent integrationEvent)
            => HandleInternal(integrationEvent, "Your order has been canceled successfully.");
    }
}
