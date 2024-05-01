using FluentResults;
using FluentValidation;

namespace WebStore.Application.Clients.Commands.CreateClient
{
    public sealed class CreateClientValidator : AbstractValidator<RegisterClientRequest>
    {
        public CreateClientValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.PhoneNumber).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}
