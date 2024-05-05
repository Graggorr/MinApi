using WebStore.EventBus;

namespace WebStore.Domain
{
    public class ClientEvent : IntegrationEvent
    {
        public ClientEvent(string clientId, string name, string phoneNumber, string email, string routeKey, string queueName, string orders)
        {
            ClientId = clientId;
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
            Orders = orders;
            RouteKey = routeKey;
            QueueName = queueName;
        }

        public string ClientId { get; }
        public string Name { get; }
        public string PhoneNumber { get; }
        public string Email { get; }
        public string Orders { get; }
    }
}
