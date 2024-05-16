using FluentValidation;
using Integration.Orchestrator.Backend.Api.Infrastructure.Extensions;
using Integration.Orchestrator.Backend.Api.SeedWork;
using Integration.Orchestrator.Backend.Application;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Api.Infrastructure.ServiceRegistrations.Application
{
    /// <summary>
    /// Register Mediatr Services Lifetime
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RegisterMediatR : IServiceRegistration
    {
        /// <summary>
        /// Loading the lifetime of the services's MediaTr Components.
        /// </summary>
        /// <param name="services"></param>  
        /// <param name="configuration"></param>  
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {

            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
            });

            services.AddSingleton(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();
        }
    }
}
