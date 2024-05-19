using System.Text.Json.Serialization;

namespace WebStore.EventBus.Abstraction;

public abstract class IntegrationEvent
{
    public IntegrationEvent()
    {
        
    }

    public IntegrationEvent(string id, string routeKey, string queueName)
    {
        Id = id;
        CreationTimeUtc = DateTime.UtcNow;
        RouteKey = routeKey;
        QueueName = queueName;
    }


    public string Id { get; init; }
    public DateTime CreationTimeUtc { get; init; }

    [JsonIgnore()]
    public string RouteKey { get; protected set; }
    [JsonIgnore()]
    public string QueueName { get; protected set; }
    [JsonIgnore()]
    public bool IsProcessed { get; set; } = false;
}
