using FluentResults;
using WebStore.Domain;

namespace WebStore.Infrastructure.Phones;

public class PhonesRepository : IPhonesRepository
{
    public Task<Result<Phone>> GetPhoneAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> IsNumberUniqueAsync(string phone)
    {
        throw new NotImplementedException();
    }

}