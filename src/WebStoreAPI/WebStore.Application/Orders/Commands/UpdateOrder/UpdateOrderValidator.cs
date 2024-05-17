using FluentValidation;

namespace WebStore.API.Application.Orders.Commands.CreateOrder
{
    public class UpdateOrderValidator: AbstractValidator<UpdateOrderRequest>
    {
        public UpdateOrderValidator()
        {
            RuleFor(x => x.OrderId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Price).NotEmpty();
            RuleFor(x => x.ClientId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}
