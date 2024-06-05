using FluentResults;
using MediatR;
using WebStore.API.Infrastructure.Clients;

namespace WebStore.API.Application.Clients.Queries
{
    public class GetPaginatedClientsRequestHandler(IClientRepository repository)
        : IRequestHandler<GetPaginatedClientsRequest, Result<IEnumerable<ClientData>>>
    {
        private readonly IClientRepository _repository = repository;

        public async Task<Result<IEnumerable<ClientData>>> Handle(GetPaginatedClientsRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetPaginatedClients(request.Page);

            if (result.IsSuccess)
            {
                var list = new List<ClientData>();

                foreach(var client in result.Value)
                {
                    list.Add(new(client.Id, client.Name, client.PhoneNumber, client.Email));
                }

                return Result.Ok(list as IEnumerable<ClientData>);
            }

            return Result.Fail(result.Errors);
        }
    }
}
