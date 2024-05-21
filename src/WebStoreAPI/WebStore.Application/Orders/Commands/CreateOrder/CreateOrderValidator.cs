using FluentValidation;

namespace WebStore.API.Application.Orders.Commands.CreateOrder
{
    public class CreateOrderValidator: AbstractValidator<RegisterOrderRequest>
    {
        public CreateOrderValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
