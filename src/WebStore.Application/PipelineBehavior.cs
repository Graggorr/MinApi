using FluentResults;
using FluentValidation;
using MediatR;
using System.Text;
using WebStore.Infrastructure;

namespace WebStore.Application
{
    public class PipelineBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators, IEnumerable<IEventProcesser> eventProcessers)
        : IPipelineBehavior<TRequest, TResponse> where TRequest : IBaseRequest
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators = validators;
        private readonly IEnumerable<IEventProcesser> _eventProcessers = eventProcessers;

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

            var requestResult = await next();
            var result = requestResult.ToResult();

            if (result.IsSuccess)
            {
                foreach (var eventProcesser in _eventProcessers)
                {
                    eventProcesser.CallProcessing();
                }
            }

            return requestResult;
        }
    }
}
