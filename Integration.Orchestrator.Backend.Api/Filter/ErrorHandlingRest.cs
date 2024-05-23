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

            ExceptionType(exception, out string typeError, out int httpCode);

            string detail = exception.InnerException?.Message ?? exception.Message;

            // Log the exception details
            _logger.LogError(exception, "An exception occurred: {Message}, HTTP Code: {HttpCode}, Detail: {Detail}, StackTrace: {StackTrace}, Source: {Source}",
                            typeError, httpCode, detail, exception.StackTrace, exception.Source);

            ObjectResult result = new ObjectResult(new { Message = new { Code = httpCode, TypeError = typeError, Message = new List<string> { detail } } }) ;
            
            context.Result = result;
            context.HttpContext.Response.StatusCode = httpCode;
        }

        private static void ExceptionType(Exception exception, out string typeError, out int code)
        {
            switch (exception)
            {
                case ArgumentException _:
                    typeError = AppMessages.Exception_ArgumentException;
                    code = (int)HttpStatusCode.NotFound;
                    break;
               
                case OrchestratorException _:
                    typeError = AppMessages.Exception_IntegrationException;
                    code = (int)HttpStatusCode.BadRequest;
                    break;

                case FrontEndException _:
                    typeError = AppMessages.Exception_IntegrationException;
                    code = (int)HttpStatusCode.Conflict;
                    break;

                // When Produce Not Controled Exception
                default:
                    typeError = AppMessages.Exception_UnexpectedException;
                    code = (int)HttpStatusCode.InternalServerError;
                    break;
            }
        }
    }
}
