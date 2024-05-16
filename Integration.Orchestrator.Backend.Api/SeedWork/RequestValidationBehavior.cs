using FluentValidation;
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
        public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var errors = _validators
                .Select(v => v.Validate(request))
                .SelectMany(result => result.Errors)
                .Where(f => f != null)
                .ToList();

            //if (errors.Any())
            //{
            //    var errorDetails = new List<ErrorDetail>();
            //    foreach (var error in errors)
            //    {
            //        var errorDetail = new ErrorDetail();
            //        errorDetail.Params.Add(error.PropertyName);
            //        if (error.ErrorMessage.Contains('|'))
            //        {
            //            var messages = error.ErrorMessage.Split("|");
            //            errorDetail.Message = messages[0];
            //            errorDetail.Params.AddRange(messages.Skip(1).ToList());
            //        }
            //        else
            //        {
            //            errorDetail.Message = error.ErrorMessage;
            //        }

            //        errorDetails.Add(errorDetail);
            //    }

            //    //throw new InvalidRequestException(string.Empty, errorDetails);
            //}

            return next();
        }
    }
}
