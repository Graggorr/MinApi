using FluentResults;
using MediatR;
using WebStore.Domain;

namespace WebStore.Application.Clients.Queries;

public record GetClientHandlingRequest(Guid Id) : IRequest<Result<Client>>;
public record GetAllClientsHandlingRequest() : IRequest<Result<IEnumerable<Client>>>;