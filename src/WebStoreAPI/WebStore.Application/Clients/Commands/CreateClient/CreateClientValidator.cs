using FluentResults;
using FluentValidation;
using WebStore.API.Application.Clients.Commands;

namespace WebStore.API.Application.Clients.Commands.CreateClient
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
