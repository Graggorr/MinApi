using System.Text.Json;
using System.Text.Json.Serialization;
using WebStore.API.Clients;
using WebStore.Infrastructure.RabbitMq;

namespace WebStore.API
{
    [JsonSourceGenerationOptions(
    JsonSerializerDefaults.Web,
    UseStringEnumConverter = true,
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    [JsonSerializable(typeof(PostClientRequestBody))]
    [JsonSerializable(typeof(PostClientResponse))]
    [JsonSerializable(typeof(PutClientRequest))]
    [JsonSerializable(typeof(PutClientRequestBody))]
    [JsonSerializable(typeof(PutClientResponse))]
    [JsonSerializable(typeof(GetClientRequest))]
    [JsonSerializable(typeof(GetClientResponse))]
    [JsonSerializable(typeof(GetAllClientsResponse))]
    [JsonSerializable(typeof(DeleteClientRequest))]
    [JsonSerializable(typeof(RabbitMqConfiguration))]
    internal partial class JsonContext : JsonSerializerContext
    {
    }
}
