using FluentResults;
using MediatR;

namespace WebStore.API.Application.Clients.Queries;

public record GetClientRequest(Guid Id) : IRequest<Result<ClientData>>;
public record GetPaginatedClientsRequest(int Page) : IRequest<Result<IEnumerable<ClientData>>>;