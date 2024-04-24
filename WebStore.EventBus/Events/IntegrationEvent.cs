using System.Text.Json.Serialization;

namespace WebStore.EventBus.Events;

public abstract class IntegrationEvent
{
    [JsonIgnore()]
    public abstract string Postfix { get; }
}
