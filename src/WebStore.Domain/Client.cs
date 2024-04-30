namespace WebStore.Domain
{
    public class Client
    {
        public required Guid Id { get; init; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<Order> Orders { get; init; }
    }
}