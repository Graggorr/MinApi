using FluentResults;
using MediatR;
using WebStore.API.Infrastructure.Clients;
using WebStore.API.Domain;

namespace WebStore.API.Application.Clients.Commands.CreateClient
{
    public class CreateClientRequestHandler(IClientRepository clientRepository) : IRequestHandler<RegisterClientRequest, Result<Guid>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task<Result<Guid>> Handle(RegisterClientRequest request, CancellationToken cancellationToken)
        {
            var businessValidationResult = await ClientBusinessValidator.BusinessValidationAsync(_clientRepository, request);

            if (businessValidationResult.IsFailed)
            {
                return businessValidationResult;
            }

            var client = new Client { Id = request.Id, Email = request.Email, Name = request.Name, PhoneNumber = request.PhoneNumber, Orders = [] };

            var result = await _clientRepository.AddClientAsync(client);

            if (result.IsSuccess)
            {
                return Result.Ok(client.Id);
            }

            return result;
        }
    }
}
