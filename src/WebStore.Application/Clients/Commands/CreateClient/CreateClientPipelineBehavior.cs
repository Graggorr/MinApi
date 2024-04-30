using FluentResults;
using FluentValidation;
using MediatR;
using System.Text;
using WebStore.Domain;

namespace WebStore.Application.Clients.Commands.CreateClient
{
    public class CreateClientPipelineBehavior(IClientRepository clientRepository, IOrderRepository orderRepository,
        IValidator<RegisterClientRequest> validator) : IPipelineBehavior<RegisterClientRequest, Result<Guid>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IValidator<RegisterClientRequest> _validator = validator;

        public async Task<Result<Guid>> Handle(RegisterClientRequest request, RequestHandlerDelegate<Result<Guid>> next, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var stringBuilder = new StringBuilder();
                validationResult.Errors.ForEach(error => stringBuilder.AppendLine(error.ErrorMessage));

                return Result.Fail(stringBuilder.ToString());
            }

            var businessValidationResult = await _validator.BusinessValidationAsync(_clientRepository, _orderRepository, request.Id);

            if (businessValidationResult.IsFailed)
            {
                return Result.Fail(businessValidationResult.Errors);
            }

            return await next();
        }
    }
}
