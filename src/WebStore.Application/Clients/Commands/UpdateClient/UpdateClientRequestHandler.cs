using FluentResults;
using MediatR;
using System.Transactions;
using WebStore.Domain;
using WebStore.EventBus;
using WebStore.Infrastructure.Clients;

namespace WebStore.Application.Clients.Commands.UpdateClient
{
    public class UpdateClientRequestHandler(IClientRepository clientRepository): IRequestHandler<UpdateClientRequest, Result<Client>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;

        public async Task<Result<Client>> Handle(UpdateClientRequest request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;
            var businessValidationResult = await ClientBusinessValidator.BusinessValidationAsync(_clientRepository, dto);

            if (businessValidationResult.IsFailed)
            {
                return businessValidationResult;
            }

            var client = new Client { Id = dto.Id, Name = dto.Name, Email = dto.Email, PhoneNumber = dto.PhoneNumber, Orders = [] };

            return await _clientRepository.UpdateClientAsync(client);
        }
    }
}
