using FluentValidation;
using MediatR;
using System.Text;

namespace WebStore.Application
{
    public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest
    {
        private readonly IEnumerable<IValidator> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validator = _validators.FirstOrDefault(x => x.GetType().GenericTypeArguments.FirstOrDefault(y => y.Equals(typeof(TRequest))) != null);

            if (validator != null)
            {
                var validationContext = new ValidationContext<TRequest>(request);
                var validationResult = await validator.ValidateAsync(validationContext, cancellationToken);

                if (!validationResult.IsValid)
                {
                    var stringBuilder = new StringBuilder();
                    validationResult.Errors.ForEach(x => stringBuilder.AppendLine(x.ErrorMessage));

                    throw new ValidationException(stringBuilder.ToString());
                }
            }

            return await next();
        }
    }
}
