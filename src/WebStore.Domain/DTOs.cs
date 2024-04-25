using FluentResults;
using MediatR;

namespace WebStore.Domain;

public record ClientDto(Guid Id, string Name, string PhoneNumber, string Email, IList<OrderDto> Orders): IRequest<Result<Client>>;
public record OrderDto(int Id, string Name, string Description, double Price, IList<Client> Clients): IRequest<Result<Order>>;
