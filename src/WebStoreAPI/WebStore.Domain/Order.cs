using System.Text.Json.Serialization;

namespace WebStore.API.Domain
{
    public class Order
    {
        public int Id { get; init; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required string Description { get; set; }
        [JsonIgnore()]
        public required Guid ClientId { get; init; }
        [JsonIgnore()]
        public required Client Client { get; init; }

        public string ToStringEmailMessage() => $"\n{Name} - {Price}";
        public string ToStringWithoutClients() => $"id: {Id}\nname: {Name}\nprice: {Price}\ndescription: {Description}";
    }
}
