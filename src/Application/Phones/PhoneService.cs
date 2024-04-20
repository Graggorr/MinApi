using FluentResults;
using Riok.Mapperly.Abstractions;
using WebStore.Infrastructure.Phones;
using WebStore.Domain;

namespace Application.Phones;

public class PhoneService(IPhonesRepository repository, PhoneMapper mapper) : IPhonesService
{
    //CQRS
    private readonly IPhonesRepository _repository = repository;
    private readonly PhoneMapper _mapper = mapper;

    public async Task<Result<Phone>> CreatePhone(string number)
    {
        var phone = await Phone.CreatePhoneAsync(number, _repository);

        phone.Value.SendSms();

        return Result.Ok(phone.Value);
    }

    public async Task<Result<Phone>> GetPhoneAsync(int id)
    {
        var phone = await _repository.GetPhoneAsync(id);

        return Result.Ok(phone.Value);
    }
}

[Mapper]
public partial class PhoneMapper
{

}