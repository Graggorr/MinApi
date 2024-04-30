using FluentResults;
using FluentValidation;
using MediatR;
using System.Text;
using WebStore.Domain;

namespace WebStore.Application.Clients.Commands.CreateClient
{
    public class CreateClientPipelineBehavior(IClientRepository clientRepository, IOrderRepository orderRepository,
        IValidator<PostClientHandlingRequest> validator) : IPipelineBehavior<PostClientHandlingRequest, Result<Client>>
    {
        private readonly IClientRepository _clientRepository = clientRepository;
        private readonly IOrderRepository _orderRepository = orderRepository;
        private readonly IValidator<PostClientHandlingRequest> _validator = validator;

        public async Task<Result<Client>> Handle(PostClientHandlingRequest request, RequestHandlerDelegate<Result<Client>> next, CancellationToken cancellationToken)
        {
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                var stringBuilder = new StringBuilder();
                validationResult.Errors.ForEach(error => stringBuilder.AppendLine(error.ErrorMessage));

                return Result.Fail(stringBuilder.ToString());
            }

            var businessValidationResult = await ClientValidator.BusinessValidationAsync(_clientRepository, _orderRepository, request.Dto);

            if (businessValidationResult.IsFailed)
            {
                return Result.Fail(businessValidationResult.Errors);
            }

            return await next();
        }
    }
}
