using FluentResults;
using MediatR;
using System.Transactions;
using WebStore.Domain;
using WebStore.EventBus;
using WebStore.Infrastructure.RabbitMq.Events;

namespace WebStore.Application.Clients
{
    public class PutClientRequestHandler(IClientRepository clientRepository, IOrderRepository orderRepository, IEventBus eventBus) :
        IRequestHandler<PutClientHandlingRequest, Result<Client>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IEventBus _eventBus = eventBus;

        public async Task<Result<Client>> Handle(PutClientHandlingRequest request, CancellationToken cancellationToken)
        {
            var verifyPhoneNumber = false;
            var verifyEmail = false;
            var dto = request.Dto;

            if (!string.IsNullOrEmpty(dto.PhoneNumber))
            {
                verifyPhoneNumber = true;
            }

            if (!string.IsNullOrEmpty(dto.Email))
            {
                verifyEmail = true;
            }

            var clientResult = await Client.CreateClientAsync(dto, _clientRepository, _orderRepository, verifyPhoneNumber, verifyEmail);

            if (clientResult.IsFailed)
            {
                return Result.Fail(clientResult.Errors);
            }

            var client = clientResult.Value;
            var integrationEvent = ClientEvent.CreateIntegrationEvent<ClientUpdatedEvent>(client);

            try
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    Task.WaitAll([_clientRepository.UpdateClientAsync(client), _eventBus.PublishAsync(integrationEvent)], cancellationToken);
                    transaction.Complete();
                }

                return Result.Ok(client);
            }
            catch (Exception exception)
            {
                return Result.Fail(exception.Message);
            }
        }
    }
}
