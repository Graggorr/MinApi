using FluentResults;
using WebStore.Domain;

namespace Application.Phones;

public interface IPhonesService
{
    Task<Result<Phone>> GetPhoneAsync(int id);

    Task<Result<Phone>> CreatePhone(string phone);
}