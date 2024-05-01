using FluentResults;
using MediatR;
using WebStore.Domain;
using WebStore.Infrastructure.Clients;

namespace WebStore.Application.Clients.Commands.CreateClient
{
    public class CreateClientRequestHandler(IClientRepository clientRepository): IRequestHandler<RegisterClientRequest, Result<Guid>>
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

            return await _clientRepository.AddClientAsync(client);
        }
    }
}
