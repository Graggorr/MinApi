using FluentResults;
using MediatR;
using WebStore.API.Domain;
using WebStore.API.Infrastructure.Clients;

namespace WebStore.API.Application.Clients.Queries
{
    public class GetPaginatedClientsRequestHandler(IClientRepository repository) : IRequestHandler<GetPaginatedClientsRequest, Result<IEnumerable<Client>>>
    {
        private readonly IClientRepository _repository = repository;

        public Task<Result<IEnumerable<Client>>> Handle(GetPaginatedClientsRequest request, CancellationToken cancellationToken)
           => Task.FromResult(_repository.GetPaginatedClients(request.Page));
    }
}
