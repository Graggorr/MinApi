using FluentResults;
using MediatR;
using System.Transactions;
using WebStore.Domain;
using WebStore.EventBus;
using WebStore.Infrastructure.RabbitMq.Events;

namespace WebStore.Application.Clients.Commands.CreateClient
{
    public class CreateClientRequestHandler(IClientRepository clientRepository, IEventBus eventBus)
        : IRequestHandler<RegisterClientRequest, Result<Guid>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<Result<Guid>> Handle(RegisterClientRequest request, CancellationToken cancellationToken)
        {
            await _clientRepository.IsEmailUniqueAsync(request.Email);
            await _clientRepository.IsPhoneNumberUniqueAsync(request.PhoneNumber);

            var client = new Client { Id = request.Id };

            var integrationEvent = ClientEvent.CreateIntegrationEvent<ClientCreatedEvent>(client);

            await _eventBus.PublishAsync(integrationEvent);

            return Result.Ok(client.Id);

        }
    }
}
