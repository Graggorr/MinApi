using WebStore.EventBus.Abstraction;

namespace WebStore.Events.Clients
{
    public class ClientEvent : IntegrationEvent
    {
        public ClientEvent() : base() { }
        public ClientEvent(string id, string name, string phoneNumber, string email,
            string routeKey, string queueName) : base(id, routeKey, queueName)
        {
            Name = name;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public string Name { get; init; }
        public string PhoneNumber { get; init; }
        public string Email { get; init; }
    }
}
