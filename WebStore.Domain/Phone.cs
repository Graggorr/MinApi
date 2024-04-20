using FluentResults;
using WebStore.Infrastructure.Phones;

namespace WebStore.Domain;

public class Phone
{
    private Phone() { }

    string PhoneNumber { get; init; }

    public static async Task<Result<Phone>> CreatePhoneAsync(string number, IPhonesRepository repository)
    {
        if (!await repository.IsNumberUniqueAsync(number))
        {
            return Result.Fail(new Error("Number already exists"));
        }

        var phone = new Phone { PhoneNumber = number };

        return phone;
    }

    public void SendSms()
    {
        throw new NotImplementedException();
    }
}