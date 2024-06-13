using FluentResults;
using MediatR;

namespace WebStore.API.Application.Clients.Commands;
public record RegisterClientRequest(ClientData Client) : IRequest<Result<Guid>>;
public record UpdateClientRequest(ClientData Client) : IRequest<Result<ClientData>>;
public record DeleteClientRequest(Guid Id) : IRequest<Result<ClientData>>;