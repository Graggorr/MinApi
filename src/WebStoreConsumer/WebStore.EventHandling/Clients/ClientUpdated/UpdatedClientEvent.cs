using WebStore.Events.Clients;

namespace WebStore.Consumer.RabbitMq.Clients.ClientUpdated
{
    public class UpdatedClientEvent : ClientEvent
    {
        public UpdatedClientEvent() : base() { }

        public UpdatedClientEvent(string id, string name, string phoneNumber, string email, string routeKey, string queueName)
            : base(id, name, phoneNumber, email, routeKey, queueName) { }
    }
}
