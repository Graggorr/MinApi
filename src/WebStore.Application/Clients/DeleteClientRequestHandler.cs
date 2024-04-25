using FluentResults;
using MediatR;
using WebStore.Domain;

namespace WebStore.Application.Clients
{
    public class DeleteClientRequestHandler(IClientRepository repository) : IRequestHandler<DeleteClientHandlingRequest, Result>
    {
        private readonly IClientRepository _repository = repository;

        public async Task<Result> Handle(DeleteClientHandlingRequest request, CancellationToken cancellationToken)
        {
            var result = await _repository.DeleteClientAsync(request.Id);

            if (!result)
            {
                return Result.Fail($"{request.Id} is not found");
            }

            return Result.Ok();
        }
    }
}
