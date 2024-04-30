namespace WebStore.Domain;
public record ClientSwaggerDto(Guid Id, string Name, string PhoneNumber, string Email);
public record OrderSwaggerDto(int Id, string Name, string Description, double Price);
public record OrderDto(int Id, string Name, string Description, double Price, List<ClientSwaggerDto> Clients);
