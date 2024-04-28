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
        public List<Order> Orders { get; set; }

        public void SendEmailMessageAfterOrderReceiving()
        {
            //var emailAddress = new MailAddress("Email");
        }

        public string ToStringWithoutId() => $"Name: {Name}\nPhoneNumber: {PhoneNumber}\nEmail: {Email}";

        public static async Task<Result<Client>> CreateClientAsync(ClientDto dto, IClientRepository clientRepository, IOrderRepository orderRepository,
            bool verifyUniquePhoneNumber, bool verifyUniqueEmail)
        {
            if (verifyUniquePhoneNumber)
            {
                var result = await clientRepository.IsPhoneNumberUniqueAsync(dto.PhoneNumber);

                if (!result)
                {
                    return Result.Fail($"{dto.PhoneNumber} is already used");
                }
            }

            if (verifyUniqueEmail)
            {
                var result = await clientRepository.IsEmailUniqueAsync(dto.Email);

                if (!result)
                {
                    return Result.Fail($"{dto.Email} is already used");
                }
            }

            if (dto.Orders?.Count == 0)
            {
                return Result.Fail(new Error("The order container is empty"));
            }

            var orders = new List<Order>();

            if (dto.Orders?.Count != 0)
            {
                foreach (var order in dto.Orders)
                {
                    var result = await orderRepository.GetOrderAsync(order.Id);

                    if (result.IsSuccess)
                    {
                        orders.Add(result.Value);
                    }
                }
            }

            Guid id;

            if (dto.Id == default || dto.Id.Equals(Guid.Empty))
            {
                id = Guid.NewGuid();
            }
            else
            {
                id = dto.Id;
            }

            var client = new Client { Id = id, PhoneNumber = dto.PhoneNumber, Email = dto.Email, Name = dto.Name, Orders = orders };

            return client;
        }
    }
}