using WebStore.Events.Orders;

namespace WebStore.Consumer.RabbitMq.Orders.OrderDeleted
{
    public class DeletedOrderEvent: OrderEvent
    {
        public DeletedOrderEvent() : base() { }

        public DeletedOrderEvent(string id, string clientId, string name, string description,
            double price, string routeKey, string queueName) : base(id, clientId, name, description, price, routeKey, queueName) { }
    }
}