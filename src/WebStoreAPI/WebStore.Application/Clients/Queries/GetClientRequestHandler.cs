using FluentResults;
using MediatR;
using WebStore.API.Infrastructure.Clients;

namespace WebStore.API.Application.Clients.Queries
{
    public class GetClientRequestHandler(IClientRepository repository) : IRequestHandler<GetClientRequest, Result<ClientData>>
    {
        private readonly IClientRepository _repository = repository;

        public async Task<Result<ClientData>> Handle(GetClientRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetClientAsync(request.Id);

            if (result.IsSuccess)
            {
                var client = result.Value;

                return Result.Ok(new ClientData(client.Id, client.Name, client.PhoneNumber, client.Email));
            }

            return Result.Fail(result.Errors);
        }
    }
}
