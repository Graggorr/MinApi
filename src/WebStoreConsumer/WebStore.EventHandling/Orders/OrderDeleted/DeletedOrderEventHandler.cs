using FluentResults;
using Microsoft.Extensions.Logging;
using System.Data;
using WebStore.EventBus.Abstraction;

namespace WebStore.Consumer.RabbitMq.Orders.OrderDeleted
{
    public class DeletedOrderEventHandler(IDbConnection connection, ILogger<IIntegrationEventHandler<DeletedOrderEvent>> logger)
        : OrderEventHandler<DeletedOrderEvent>(connection, logger)
    {
        public override Task<Result> Handle(DeletedOrderEvent integrationEvent)
            => HandleInternal(integrationEvent, "Your order has been canceled successfully.");
    }
}
