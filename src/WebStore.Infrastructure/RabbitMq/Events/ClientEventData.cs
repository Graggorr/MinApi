namespace WebStore.Infrastructure.RabbitMq.Events
{
    public record class ClientEventData(string ClientId, string Name, string PhoneNumber, string Email, List<string> Orders);
}
