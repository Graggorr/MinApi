using FluentResults;
using MediatR;
using WebStore.API.Domain;
using WebStore.API.Infrastructure.Clients;

namespace WebStore.API.Application.Clients.Commands.DeleteClient
{
    public class DeleteClientRequestHandler(IClientRepository repository) : IRequestHandler<DeleteClientRequest, Result<Client>>
    {
        private readonly IClientRepository _repository = repository;

        public async Task<Result<Client>> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
            => await _repository.DeleteClientAsync(request.Id);
    }
}