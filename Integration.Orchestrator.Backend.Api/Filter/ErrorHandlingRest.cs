using Integration.Orchestrator.Backend.Application.Exceptions;
using Integration.Orchestrator.Backend.Application.Models;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Exceptions;
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
        /// <summary>
        /// Method that is called when the API produces an Exception
        /// </summary>    
        public override void OnException(ExceptionContext context)
        {
            var exception = context.Exception;
            context.ExceptionHandled = true;

            ExceptionType(exception, out int httpCode);

            var detail = exception.InnerException?.Message ?? exception.Message;

            ObjectResult? result = exception switch
            {
                InvalidRequestException invalidRequestException => new ObjectResult(new
                {
                    Code = ResponseCode.NotValidationSuccessfully,
                    Messages = invalidRequestException.Details.Messages.Select(m => m).ToList(),
                    invalidRequestException.Details.Data
                }),
                OrchestratorArgumentException orchestratorArgumentException => new ObjectResult(new
                {
                    Code = orchestratorArgumentException?.Details?.Code,
                    Messages = new string?[] { orchestratorArgumentException?.Details?.Description },
                    orchestratorArgumentException?.Details?.Data
                }),
                _ => new ObjectResult(new ErrorResponse { Code = httpCode, Messages = [detail] })
            };

            context.Result = result;
            context.HttpContext.Response.StatusCode = httpCode;
        }

        private static void ExceptionType(Exception exception, out int code)
        {
            switch (exception)
            {
                case InvalidRequestException:
                    code = (int)HttpStatusCode.BadRequest;
                    break;

                case OrchestratorArgumentException:
                    code = (int)HttpStatusCode.NotFound;
                    break;

                case OrchestratorException:
                    code = (int)HttpStatusCode.BadRequest;
                    break;

                case FrontEndException:
                    code = (int)HttpStatusCode.Conflict;
                    break;

                default:
                    code = (int)HttpStatusCode.InternalServerError;
                    break;
            }
        }
    }
}
