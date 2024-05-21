using WebStore.Events.Orders;

namespace WebStore.Consumer.RabbitMq.Orders.OrderUpdated
{
    public class UpdatedOrderEvent : OrderEvent
    {
        public UpdatedOrderEvent() : base() { }

        public UpdatedOrderEvent(string id, string clientId, string name, string description,
            double price, string routeKey, string queueName) : base(id, clientId, name, description, price, routeKey, queueName) { }
    }
}
