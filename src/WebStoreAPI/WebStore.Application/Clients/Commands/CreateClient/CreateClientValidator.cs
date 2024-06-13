using FluentValidation;

namespace WebStore.API.Application.Clients.Commands.CreateClient
{
    public sealed class CreateClientValidator : AbstractValidator<RegisterClientRequest>
    {
        public CreateClientValidator()
        {
            RuleFor(x => x.Client.Name).NotEmpty();
            RuleFor(x => x.Client.PhoneNumber).NotEmpty();
            RuleFor(x => x.Client.Email).NotEmpty();
        }
    }
}
