using FluentResults;
using MediatR;
using WebStore.Domain;

namespace WebStore.Application.Clients.Commands;
public record RegisterClientRequest(Guid Id, string Name, string PhoneNumber, string Email) : IRequest<Result<Guid>>;
public record UpdateClientRequest(RegisterClientRequest Dto) : IRequest<Result<Client>>;
public record DeleteClientHandlingRequest(Guid Id) : IRequest<Result<Client>>;

