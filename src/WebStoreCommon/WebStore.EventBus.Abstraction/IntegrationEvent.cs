using System.Text.Json.Serialization;

namespace WebStore.EventBus.Abstraction;

public abstract class IntegrationEvent
{
    public string Id { get; init; }

    [JsonIgnore()]
    public string RouteKey { get; protected set; }
    [JsonIgnore()]
    public string QueueName { get; protected set; }
    [JsonIgnore()]
    public bool IsProcessed { get; set; } = false;
}
