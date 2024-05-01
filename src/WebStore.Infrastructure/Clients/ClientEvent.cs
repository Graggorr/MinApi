using WebStore.EventBus;

namespace WebStore.Infrastructure.RabbitMq.Events
{
    public class ClientEvent(string clientId, string name, string phoneNumber, string email, string routeKey, string queueName, string orders) : IntegrationEvent
    {
        public string ClientId => clientId;
        public string Name => name;
        public string PhoneNumber => phoneNumber;
        public string Email => email;
        public string Orders => orders;
        public override string RouteKey => routeKey;
        public override string QueueName => queueName;
    }
}
