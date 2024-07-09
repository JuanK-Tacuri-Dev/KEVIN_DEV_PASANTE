using FluentValidation;
using Integration.Orchestrator.Backend.Application.Exceptions;
using Integration.Orchestrator.Backend.Application.Models;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Api.SeedWork
{
    /// <summary>
    /// Fluent Validation Behavior class to managment the Fluent Validations
    /// </summary>    
    [ExcludeFromCodeCoverage]
    public class RequestValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        /// <summary>
        /// Handle to validate request input and check errors
        /// </summary>
        /// <param name="validators">Request to validate</param>      
        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// Handle to validate request input and check errors
        /// </summary>
        /// <param name="request"> Request to validate</param>
        /// <param name="cancellationToken">MediatR Cancelation Token</param>
        /// <param name="next">Delegate response</param>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var errors = _validators
                .SelectMany(v => v.Validate(request).Errors)
                .Where(f => f != null)
                .ToList();

            if (errors.Any())
            {
                var errorDetails = errors.Select(error =>
                {
                    var obj = error.PropertyName.Split(".");
                    var errorDetail = new ErrorDetail
                    {

                        Params = obj[obj.Length - 1],
                        Message = error.ErrorMessage
                    };
                    return errorDetail;
                }).Distinct().ToList();

                throw new InvalidRequestException(string.Empty, errorDetails);
            }

            return await next();
        }
    }
}
