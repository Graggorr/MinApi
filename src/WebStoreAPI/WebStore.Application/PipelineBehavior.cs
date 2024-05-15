using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Text;
using WebStore.API.Infrastructure;

namespace WebStore.API.Application
{
    public class PipelineBehavior<TRequest, TResponse>(ILogger<IPipelineBehavior<TRequest, TResponse>> logger, IEnumerable<IValidator<TRequest>> validators)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
        private readonly ILogger _logger = logger;

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogDebug($"Validation for {typeof(TRequest).Name} called");

            var validationContext = new ValidationContext<TRequest>(request);
            var validationResult = _validators.Select(v => v.Validate(validationContext)).SelectMany(result => result.Errors)
                .Where(failure => failure is not null).ToList();

            if (validationResult is not null && validationResult.Count != 0)
            {
                var stringBuilder = new StringBuilder();
                validationResult.ForEach(x => stringBuilder.AppendLine(x.ErrorMessage));
                var exception = new ValidationException(stringBuilder.ToString());
                _logger.LogException(exception);

                throw exception;
            }

            return await next();
        }
    }
}
