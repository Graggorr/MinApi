using WebStore.Events.Clients;

namespace WebStore.Consumer.RabbitMq.ClientRegistered
{
    public class CreatedClientEvent(string clientId, string name, string phoneNumber, string email, string routeKey, string queueName)
        : ClientEvent(clientId, name, phoneNumber, email, routeKey, queueName)
    { }
}
