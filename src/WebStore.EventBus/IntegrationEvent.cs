using System.Text.Json.Serialization;

namespace WebStore.EventBus;

public abstract class IntegrationEvent
{
    [JsonIgnore()]
    public string RouteKey { get; protected set; }
    [JsonIgnore()]
    public string QueueName { get; protected set; }
    [JsonIgnore()]
    public bool IsProcessed { get; set; } = false;
}
