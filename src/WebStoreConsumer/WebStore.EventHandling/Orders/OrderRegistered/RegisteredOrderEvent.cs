using WebStore.Events.Orders;

namespace WebStore.Consumer.RabbitMq.Orders.OrderRegistered
{
    public class RegisteredOrderEvent : OrderEvent
    {
        public RegisteredOrderEvent() : base() { }

        public RegisteredOrderEvent(string id, string clientId, string name, string description,
            double price, string routeKey, string queueName) : base(id, clientId, name, description, price, routeKey, queueName) { }
    }
}
