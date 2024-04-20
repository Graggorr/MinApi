using FluentResults;
using WebStore.Domain;

namespace WebStore.Infrastructure.Phones;

public interface IPhonesRepository
{
    Task<Result<Phone>> GetPhoneAsync(int id);
    Task<bool> IsNumberUniqueAsync(string phone);
}