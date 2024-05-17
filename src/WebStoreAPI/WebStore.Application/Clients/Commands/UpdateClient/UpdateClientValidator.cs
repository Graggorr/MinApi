using FluentValidation;
using WebStore.API.Application.Clients.Commands;

namespace WebStore.API.Application.Clients.Commands.UpdateClient
{
    public sealed class UpdateClientValidator : AbstractValidator<UpdateClientRequest>
    {
        public UpdateClientValidator()
        {
            RuleFor(x => x.RequestBody.Name).NotEmpty();
            RuleFor(x => x.RequestBody.PhoneNumber).NotEmpty();
            RuleFor(x => x.RequestBody.Email).NotEmpty();
        }
    }
}
