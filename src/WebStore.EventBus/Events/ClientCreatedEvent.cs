using System.Text.Json.Serialization;

namespace WebStore.EventBus.Events;

public class ClientCreatedEvent(string clientId, string name, string phoneNumber, string email, List<string> orders): IntegrationEvent
{
    [JsonPropertyName("clientId")]
    public string ClientId { get; private set; } = clientId;
    [JsonPropertyName("name")]
    public string Name { get; private set; } = name;
    [JsonPropertyName("phoneNumber")]
    public string PhoneNumber { get; private set; } = phoneNumber;
    [JsonPropertyName("email")]
    public string Email { get; private set; } = email;
    [JsonPropertyName("orders")]
    public List<string> Orders { get; private set; } = orders;
    public override string Postfix => "user/player/customer";
}
