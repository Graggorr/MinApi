using WebStore.EventBus.Abstraction;

namespace WebStore.Events.Orders
{
    public class OrderEvent : IntegrationEvent
    {
        public OrderEvent() : base() { }
        public OrderEvent(string id, string clientId, string name, string description,
            double price, string routeKey, string queueName) : base(id, routeKey, queueName)
        {
            ClientId = clientId;
            Name = name;
            Description = description;
            Price = price;
        }

        public string ClientId { get; set; }
        public string Name { get; init; }
        public string Description { get; init; }
        public double Price { get; init; }
    }
}
