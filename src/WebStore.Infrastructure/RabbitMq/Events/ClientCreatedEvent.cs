using WebStore.EventBus;

namespace WebStore.Infrastructure.RabbitMq.Events;

public class ClientCreatedEvent(string clientId, string name, string phoneNumber, string email, List<string> orders) : IntegrationEvent
{
    public string ClientId { get; } = clientId;
    public string Name { get; } = name;
    public string PhoneNumber { get; } = phoneNumber;
    public string Email { get; } = email;
    public List<string> Orders { get; } = orders;
    public override string RouteKey => "user/player/customer";
    public override string QueueName => "client_created";
}