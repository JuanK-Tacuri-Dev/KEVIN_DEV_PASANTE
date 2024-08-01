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
    where TRequest : notnull
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
                var errorMessages = errors.Select(error =>
                {
                    var obj = error.PropertyName.Split(".");
                    var errorDetail = new Dictionary<string, string>
                    {
                        { obj[obj.Length - 1], error.ErrorMessage }
                    };
                    return errorDetail;
                }).ToList();

                throw new InvalidRequestException(string.Empty, new DetailsErrors()
                {
                    Messages = errorMessages,
                    Data = GetThirdLevelProperties(request)
                });
            }

            return await next();
        }

        private static object GetThirdLevelProperties(object obj)
        {
            var firstLevelProperties = obj.GetType().GetProperties();
            var thirdLevelProperties = firstLevelProperties.SelectMany(prop =>
            {
                var subObj = prop.GetValue(obj);
                if (subObj == null) return Enumerable.Empty<dynamic>();

                var secondLevelProperties = subObj.GetType().GetProperties();
                return secondLevelProperties.SelectMany(subProp =>
                {
                    var thirdObj = subProp.GetValue(subObj);
                    return thirdObj?.GetType().GetProperties().Select(thirdProp => new { Name = thirdProp.Name, Value = thirdProp.GetValue(thirdObj) }) ?? Enumerable.Empty<dynamic>();
                });
            }).ToDictionary(x => x.Name, x => x.Value);

            return thirdLevelProperties;
        }
    }
}
