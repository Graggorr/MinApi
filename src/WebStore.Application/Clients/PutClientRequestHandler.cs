using FluentResults;
using MediatR;
using WebStore.Domain;

namespace WebStore.Application.Clients
{
    public class PutClientRequestHandler(IClientRepository clientRepository, IOrderRepository orderRepository) : IRequestHandler<PutClientHandlingRequest, Result<Client>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;

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

            var result = await _clientRepository.UpdateClientAsync(clientResult.Value);

            if (result)
            {
                return Result.Ok();
            }

            return Result.Fail($"{clientResult.Value.Id} is not found");
        }
    }
}
