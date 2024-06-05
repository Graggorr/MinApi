using FluentValidation;

namespace WebStore.API.Application.Clients.Commands.UpdateClient
{
    public sealed class UpdateClientValidator : AbstractValidator<UpdateClientRequest>
    {
        public UpdateClientValidator()
        {
            RuleFor(x => x.Client.Name).NotEmpty();
            RuleFor(x => x.Client.PhoneNumber).NotEmpty();
            RuleFor(x => x.Client.Email).NotEmpty();
        }
    }
}
