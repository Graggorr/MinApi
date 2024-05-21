using FluentResults;
using MediatR;
using Microsoft.Extensions.Logging;
using WebStore.API.Domain;
using WebStore.API.Infrastructure.Clients;

namespace WebStore.API.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientRequestHandler(ILogger<IRequestHandler<UpdateClientRequest, Result<Client>>> logger,
        IClientRepository clientRepository) : IRequestHandler<UpdateClientRequest, Result<Client>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly ILogger _logger = logger;

        public async Task<Result<Client>> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var requestBody = request.RequestBody;
            var businessValidationResult = await ClientBusinessValidator.BusinessValidationAsync(_clientRepository, requestBody);

            if (businessValidationResult.IsFailed)
            {
                _logger.LogDebug($"{requestBody.Id} did not pass business validation.");

                return businessValidationResult;
            }

            var client = new Client { Id = requestBody.Id, Name = requestBody.Name, Email = requestBody.Email,
                PhoneNumber = requestBody.PhoneNumber, Orders = [] };

            return await _clientRepository.UpdateClientAsync(client);
        }
    }
}
