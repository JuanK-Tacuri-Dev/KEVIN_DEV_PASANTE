using FluentValidation;
using Integration.Orchestrator.Backend.Application.Exceptions;
using Integration.Orchestrator.Backend.Application.Models;
using Integration.Orchestrator.Backend.Domain.Commons;
using MediatR;
using System.Collections.Generic;
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
        /// <param key="validators">Request to validate</param>      
        public RequestValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        /// <summary>
        /// Handle to validate request input and check errors
        /// </summary>
        /// <param key="request"> Request to validate</param>
        /// <param key="cancellationToken">MediatR Cancelation Token</param>
        /// <param key="next">Delegate response</param>
        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var errors = _validators
                .SelectMany(v => v.Validate(request).Errors)
                .Where(f => f != null)
                .ToList();

            if (errors.Any())
            {
                var errorMessages = new  List<string>()
                {
                    $"{"Object:"} {ResponseMessageValues.GetResponseMessage(ResponseCode.NotValidationSuccessfully)}" 
                };
                errorMessages.AddRange( errors.Select(error =>
                {
                    var obj = error.PropertyName.Split(".");
                    
                        
                    
                    return $"{obj[obj.Length - 1]}: {error.ErrorMessage}";
                }).ToList());

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
