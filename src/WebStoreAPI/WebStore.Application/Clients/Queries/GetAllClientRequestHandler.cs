using FluentResults;
using MediatR;
using WebStore.API.Domain;
using WebStore.API.Infrastructure.Clients;

namespace WebStore.API.Application.Clients.Queries
{
    public class GetAllClientsRequestHandler(IClientRepository repository) : IRequestHandler<GetAllClientsHandlingRequest, Result<IEnumerable<Client>>>
    {
        private readonly IClientRepository _repository = repository;

        public async Task<Result<IEnumerable<Client>>> Handle(GetAllClientsHandlingRequest request, CancellationToken cancellationToken)
            => await _repository.GetAllClientsAsync();
    }
}
