using FluentResults;
using MediatR;
using WebStore.API.Domain;

namespace WebStore.API.Application.Clients.Commands;
public record RegisterClientRequest(Guid Id, string Name, string PhoneNumber, string Email) : IRequest<Result<Guid>>;
public record UpdateClientRequest(RegisterClientRequest RequestBody) : IRequest<Result<Client>>;
public record DeleteClientRequest(Guid Id) : IRequest<Result<Client>>;

