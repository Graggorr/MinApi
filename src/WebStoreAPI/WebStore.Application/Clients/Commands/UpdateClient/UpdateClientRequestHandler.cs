using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using WebStore.API.Domain;
using WebStore.API.Infrastructure.Clients;

namespace WebStore.API.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientRequestHandler(ILogger<IRequestHandler<UpdateClientRequest, Result<ClientData>>> logger,
        IClientRepository clientRepository) : IRequestHandler<UpdateClientRequest, Result<ClientData>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly ILogger _logger = logger;

        public async Task<Result<ClientData>> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var clientData = request.Client;
            var businessValidationResult = await ClientBusinessValidator.BusinessValidationAsync(_clientRepository, clientData);

            if (businessValidationResult.IsFailed)
            {
                _logger.LogDebug($"{clientData.Id} did not pass business validation.");

                return businessValidationResult;
            }

            var client = new Client { Id = clientData.Id, Name = clientData.Name, Email = clientData.Email, PhoneNumber = clientData.PhoneNumber, Orders = [] };
            var result = await _clientRepository.UpdateClientAsync(client);

            if (result.IsSuccess)
            {
                return Result.Ok(new ClientData(client.Id, client.Name, client.PhoneNumber, client.Email));
            }

            return Result.Fail(result.Errors);
        }
    }
}
