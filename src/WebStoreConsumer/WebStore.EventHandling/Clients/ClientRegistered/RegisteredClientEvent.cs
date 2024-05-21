using WebStore.Events.Clients;

namespace WebStore.Consumer.RabbitMq.Clients.ClientRegistered
{
    public class RegisteredClientEvent : ClientEvent
    {
        public RegisteredClientEvent() : base() { }

        public RegisteredClientEvent(string id, string name, string phoneNumber, string email, string routeKey, string queueName)
            : base(id, name, phoneNumber, email, routeKey, queueName) { }
    }
}
