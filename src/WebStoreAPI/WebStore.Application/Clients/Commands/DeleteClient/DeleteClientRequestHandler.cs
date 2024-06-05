using FluentResults;
using MediatR;
using WebStore.API.Domain;
using WebStore.API.Infrastructure.Clients;

namespace WebStore.API.Application.Clients.Commands.DeleteClient
{
    public class DeleteClientRequestHandler(IClientRepository repository) : IRequestHandler<DeleteClientRequest, Result<ClientData>>
    {
        private readonly IClientRepository _repository = repository;

        public async Task<Result<ClientData>> Handle(DeleteClientRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteClientAsync(request.Id);

            if (result.IsSuccess)
            {
                var client = result.Value;

                return Result.Ok(new ClientData(client.Id, client.Name, client.PhoneNumber, client.Email));
            }

            return Result.Fail(result.Errors);
        }
    }
}