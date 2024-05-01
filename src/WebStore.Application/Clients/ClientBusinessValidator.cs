using FluentResults;
using WebStore.Application.Clients.Commands;
using WebStore.Infrastructure.Clients;

namespace WebStore.Application.Clients
{
    internal class ClientBusinessValidator
    {
        public static async Task<Result> BusinessValidationAsync(IClientRepository clientRepository, RegisterClientRequest client)
        {
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
