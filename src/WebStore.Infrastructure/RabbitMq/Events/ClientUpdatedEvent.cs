namespace WebStore.Infrastructure.RabbitMq.Events;

public class ClientUpdatedEvent(string clientId, string name, string phoneNumber, string email, List<string> orders) :
    ClientEvent(clientId, name, phoneNumber, email, orders)
{
    public override string RouteKey => "user/player/customer";
    public override string QueueName => "client_updated";
}