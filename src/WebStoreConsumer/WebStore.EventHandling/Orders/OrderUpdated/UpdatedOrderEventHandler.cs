using FluentResults;
using Microsoft.Extensions.Logging;
using System.Data;
using WebStore.Consumer.RabbitMq.Orders.OrderUpdated;
using WebStore.EventBus.Abstraction;

namespace WebStore.Consumer.RabbitMq.Orders.OrderRegistered
{
    public class UpdatedOrderEventHandler(IDbConnection connection, ILogger<IIntegrationEventHandler<UpdatedOrderEvent>> logger)
        : OrderEventHandler<UpdatedOrderEvent>(connection, logger)
    {
        public override Task<Result> Handle(UpdatedOrderEvent integrationEvent)
            => HandleInternal(integrationEvent, "Your order has been canceled successfully.");
    }
}
