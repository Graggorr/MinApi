using FluentResults;
using MediatR;
using WebStore.Domain;

namespace WebStore.Application.Clients
{
    public class GetClientRequestHandler(IClientRepository repository) : IRequestHandler<GetClientHandlingRequest, Result<Client>>
    {
        private readonly IClientRepository _repository = repository;

        public async Task<Result<Client>> Handle(GetClientHandlingRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.GetClientAsync(request.Id);

            if (result.IsSuccess)
            {
                return Result.Ok(result.Value);
            }

            return Result.Fail(result.Errors);
        }
    }
}
