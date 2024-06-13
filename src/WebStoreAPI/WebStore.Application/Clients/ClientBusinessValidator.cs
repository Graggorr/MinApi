using FluentResults;
using System.Text.RegularExpressions;
using WebStore.API.Application.Clients.Commands;
using WebStore.API.Infrastructure.Clients;

namespace WebStore.API.Application.Clients
{
    internal class ClientBusinessValidator
    {
        private const string EMAIL_REGEX = "^[^@\\s]+@[^@\\s]+\\.(com|net|org|gov)$";
        private const string PHONE_NUMBER_REGEX = "^\\(?([0-9]{3})\\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";

        public static async Task<Result> BusinessValidationAsync(IClientRepository clientRepository, ClientData client)
        {
            if (!Regex.IsMatch(client.PhoneNumber, PHONE_NUMBER_REGEX))
            {
                return Result.Fail($"{client.PhoneNumber} is not valid. Valid example: 1234567890");
            }

            if (!await clientRepository.IsPhoneNumberUniqueAsync(client.PhoneNumber))
            {
                var clientResult = await clientRepository.GetClientAsync(client.Id);

                if (!clientResult.IsSuccess || !clientResult.Value.PhoneNumber.Equals(client.PhoneNumber))
                {
                    return Result.Fail("Phone number is already used");
                }

            }

            if (!Regex.IsMatch(client.Email, EMAIL_REGEX, RegexOptions.IgnoreCase))
            {
                return Result.Fail($"{client.Email} is not valid. Valid example: sample1234@gmail.com");
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
