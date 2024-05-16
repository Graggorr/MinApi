using FluentResults;
using MediatR;
using WebStore.API.Domain;
using WebStore.API.Infrastructure.Clients;

namespace WebStore.API.Application.Clients.Queries
{
    public class GetClientRequestHandler(IClientRepository repository) : IRequestHandler<GetClientHandlingRequest, Result<Client>>
    {
        private readonly IClientRepository _repository = repository;

        public async Task<Result<Client>> Handle(GetClientHandlingRequest request, CancellationToken cancellationToken)
            => await _repository.GetClientAsync(request.Id);
    }
}
