namespace WebStore.API.Domain
{
    public class Order
    {
        public required int Id { get; init; }
        public required string Name { get; set; }
        public required double Price { get; set; }
        public required string Description { get; set; }
        public required Guid ClientId { get; init; }
        public required Client Client { get; init; }

        public string ToStringEmailMessage() => $"\n{Name} - {Price}";
        public string ToStringWithoutClients() => $"id: {Id}\nname: {Name}\nprice: {Price}\ndescription: {Description}";
    }
}
