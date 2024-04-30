using FluentResults;
using FluentValidation;
using WebStore.Domain;

namespace WebStore.Application.Clients.Commands.CreateClient
{
    [Validate]
    public sealed class CreateClientValidator : AbstractValidator<RegisterClientRequest>
    {
        public CreateClientValidator()
        {
            RuleFor(x => x.Dto.Name).NotEmpty();
            RuleFor(x => x.Dto.Orders).NotEmpty();
            RuleFor(x => x.Dto.PhoneNumber).NotEmpty();
            RuleFor(x => x.Dto.Email).NotEmpty();
        }
    }
}
