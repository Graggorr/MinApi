﻿using System.Text.Json.Serialization;

namespace WebStore.EventBus;

public abstract class IntegrationEvent
{
    [JsonIgnore()]
    public abstract string RouteKey { get; }
    [JsonIgnore()]
    public abstract string QueueName { get; }
}
