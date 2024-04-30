using FluentResults;
using FluentValidation;
using WebStore.Domain;

namespace WebStore.Application.Clients.Commands
{
    public sealed class ClientValidator : AbstractValidator<PostClientHandlingRequest>
    {
        public ClientValidator()
        {
            RuleFor(x => x.Dto.Name).NotEmpty();
            RuleFor(x => x.Dto.Orders).NotEmpty();
            RuleFor(x => x.Dto.PhoneNumber).NotEmpty();
            RuleFor(x => x.Dto.Email).NotEmpty();
        }

        internal static async Task<Result> BusinessValidationAsync(IClientRepository clientRepository, IOrderRepository orderRepository, ClientDto client)
        {
            foreach (var order in client.Orders)
            {
                var result = await orderRepository.GetOrderAsync(order.Id);

                if (result.IsFailed)
                {
                    return Result.Fail(result.Errors);
                }
            }

            if (!await clientRepository.IsPhoneNumberUniqueAsync(client.PhoneNumber))
            {
                var clientResult = await clientRepository.GetClientAsync(client.Id);

                if (!clientResult.IsSuccess || !clientResult.Value.PhoneNumber.Equals(client.PhoneNumber))
                {
                    return Result.Fail("Phone number is already used");
                }

            }

            if (!await clientRepository.IsEmailUniqueAsync(client.Email))
            {
                var clientResult = await clientRepository.GetClientAsync(client.Id);

                if (!clientResult.IsSuccess || !clientResult.Value.Email.Equals(client.Email))
                {
                    return Result.Fail("Email is already used");
                }
            }

            return Result.Ok();
        }
    }
}
