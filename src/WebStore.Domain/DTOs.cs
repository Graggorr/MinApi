namespace WebStore.Domain;
public record ClientDto(Guid Id, string Name, string PhoneNumber, string Email, IList<OrderDto> Orders);
public record OrderDto(int Id, string Name, string Description, double Price, IList<ClientDto> Clients);
