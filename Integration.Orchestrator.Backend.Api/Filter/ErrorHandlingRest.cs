using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Integration.Orchestrator.Backend.Api.Filter
{
    /// <summary>
    /// Generate the handle to catching Exception in the API
    /// </summary>    
    [ExcludeFromCodeCoverage]
    public sealed class ErrorHandlingRest : ExceptionFilterAttribute
    {
        private readonly ILogger<ErrorHandlingRest> _logger;

        public ErrorHandlingRest(ILogger<ErrorHandlingRest> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Method that is called when the API produces an Exception
        /// </summary>    
        public override void OnException(ExceptionContext context)
        {
            ArgumentNullException.ThrowIfNull(context);
            Exception exception = context.Exception;
            context.ExceptionHandled = true;

            ExceptionType(exception, out string message, out int httpCode);

            string detail = exception.InnerException?.Message ?? exception.Message;

            // Log the exception details
            _logger.LogError(exception, "An exception occurred: {Message}, HTTP Code: {HttpCode}, Detail: {Detail}, StackTrace: {StackTrace}, Source: {Source}",
                            message, httpCode, detail, exception.StackTrace, exception.Source);

            ObjectResult result = new ObjectResult(new { Code = httpCode, Message = message });
            
            context.Result = result;
            context.HttpContext.Response.StatusCode = httpCode;
        }

        private static void ExceptionType(Exception exception, out string message, out int code)
        {
            switch (exception)
            {
                case ArgumentException _:
                    message = AppMessages.Exception_ArgumentException;
                    code = (int)HttpStatusCode.BadRequest;
                    break;
               
                case IntegrationException _:
                    message = AppMessages.Exception_IntegrationException;
                    code = (int)HttpStatusCode.BadRequest;
                    break;

                case FrontEndException _:
                    message = AppMessages.Exception_IntegrationException;
                    code = (int)HttpStatusCode.Conflict;
                    break;

                // When Produce Not Controled Exception
                default:
                    message = AppMessages.Exception_UnexpectedException;
                    code = (int)HttpStatusCode.InternalServerError;
                    break;
            }
        }
    }
}
