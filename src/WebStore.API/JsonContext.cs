using System.Text.Json.Serialization;
using WebStore.API.Clients;
using WebStore.Domain;

namespace WebStore.API
{
    [JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
    [JsonSerializable(typeof(PostClientRequestBody))]
    [JsonSerializable(typeof(PostClientResponse))]
    [JsonSerializable(typeof(PutClientRequest))]
    [JsonSerializable(typeof(PutClientRequestBody))]
    [JsonSerializable(typeof(PutClientResponse))]
    [JsonSerializable(typeof(GetClientRequest))]
    [JsonSerializable(typeof(GetClientResponse))]
    [JsonSerializable(typeof(GetAllClientsResponse))]
    [JsonSerializable(typeof(DeleteClientRequest))]
    internal partial class JsonContext : JsonSerializerContext
    {
    }
}
