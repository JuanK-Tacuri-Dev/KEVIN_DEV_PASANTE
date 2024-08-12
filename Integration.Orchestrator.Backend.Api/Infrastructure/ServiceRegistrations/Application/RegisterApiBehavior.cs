using Integration.Orchestrator.Backend.Api.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Integration.Orchestrator.Backend.Api.Infrastructure.ServiceRegistrations.Application
{
    [ExcludeFromCodeCoverage]
    public partial class RegisterApiBehavior : IServiceRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param key="services"></param>    
        /// <param key="configuration"></param>  
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    ValidationProblemDetails? error = actionContext.ModelState
                             .Where(e => e.Value?.Errors.Count > 0)
                             .Select(e => new ValidationProblemDetails(actionContext.ModelState))
                             .FirstOrDefault();
                    var errorField = error?.Errors.Last().Key ?? string.Empty;
                    var field = RegexField().Replace(errorField, string.Empty);
                    var errorValue = error?.Errors.Values.Last().FirstOrDefault() ?? string.Empty;
                    Match match = RegexMatch().Match(errorValue);
                    var message = match.Success ? match.Value : errorValue;
                    //throw new BadRequestException("invalid_type", $"Tipo de dato inválido: {field}.", message);
                    throw new ValidationException($"Tipo de dato inválido: {field}.");
                };
            });
        }

        [GeneratedRegex("[^A-Za-z0-9_ ]")]
        private static partial Regex RegexField();

        [GeneratedRegex(".+?(?=Path)")]
        private static partial Regex RegexMatch();
    }
}
