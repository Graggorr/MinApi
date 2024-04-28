using FluentResults;
using MediatR;
using WebStore.Domain;

namespace WebStore.Application.Clients;

public record PostClientHandlingRequest(ClientDto Dto): IRequest<Result<Client>>;
public record PutClientHandlingRequest(ClientDto Dto): IRequest<Result<Client>>;
public record DeleteClientHandlingRequest(Guid Id): IRequest<Result<Client>>;
public record GetClientHandlingRequest(Guid Id): IRequest<Result<Client>>;
public record GetAllClientsHandlingRequest(): IRequest<Result<IEnumerable<Client>>>;