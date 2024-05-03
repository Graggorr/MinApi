using FluentValidation;
using MediatR;
using System.Text;

namespace WebStore.Application
{
    public class ValidationPipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var validationContext = new ValidationContext<TRequest>(request);
            var validationResult = _validators.Select(v => v.Validate(validationContext)).SelectMany(result => result.Errors)
                .Where(failure => failure != null).ToList();

            if (validationResult != null && validationResult.Count != 0)
            {
                var stringBuilder = new StringBuilder();
                validationResult.ForEach(x => stringBuilder.AppendLine(x.ErrorMessage));

                throw new ValidationException(stringBuilder.ToString());
            }

            return await next();
        }
    }
}
