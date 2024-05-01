using FluentResults;
using MediatR;
using WebStore.Domain;
using WebStore.Infrastructure.Clients;

namespace WebStore.Application.Clients.Commands.DeleteClient
{
    public class DeleteClientRequestHandler(IClientRepository repository) : IRequestHandler<DeleteClientHandlingRequest, Result<Client>>
    {
        private readonly IClientRepository _repository = repository;

        public async Task<Result<Client>> Handle(DeleteClientHandlingRequest request, CancellationToken cancellationToken)
            => await _repository.DeleteClientAsync(request.Id);
    }
}