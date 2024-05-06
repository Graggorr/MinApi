namespace WebStore.Domain
{
    public class Client
    {
        public required Guid Id { get; init; }
        public required string Name { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required List<Order> Orders { get; init; }
    }
}