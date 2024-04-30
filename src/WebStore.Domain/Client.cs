using FluentResults;
namespace WebStore.Domain
{
    public class Client
    {
        private Client() { }

        public Guid Id { get; init; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<Order> Orders { get; init; }

        public void SendEmailMessageAfterOrderReceiving()
        {
            //var emailAddress = new MailAddress("Email");
        }

        public string ToStringWithoutId() => $"Name: {Name}\nPhoneNumber: {PhoneNumber}\nEmail: {Email}";

        public static async Task<Result<Client>> CreateClientAsync(ClientDto dto, IOrderRepository orderRepository)
        {
            var orders = new List<Order>();

            foreach (var order in dto.Orders)
            {
                var result = await orderRepository.GetOrderAsync(order.Id);

                if (result.IsSuccess)
                {
                    orders.Add(result.Value);
                }
            }

            return new Client { Id = dto.Id, PhoneNumber = dto.PhoneNumber, Email = dto.Email, Name = dto.Name, Orders = orders };
        }
    }
}