using WebStore.Events;

namespace WebStore.Consumer.RabbitMq.ClientRegistered
{
    public class CreatedClientEvent(string clientId, string name, string phoneNumber, string email, string routeKey, string queueName, string orders)
        : ClientEvent(clientId, name, phoneNumber, email, routeKey, queueName, orders)
    { }
}
