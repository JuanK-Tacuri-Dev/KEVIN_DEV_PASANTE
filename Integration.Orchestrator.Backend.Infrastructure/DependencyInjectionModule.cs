using Autofac;
using Integration.Orchestrator.Backend.Application.Options;
using Integration.Orchestrator.Backend.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Infrastructure
{
    /// <summary>
    /// Register all infrastructure related objects
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AddInfrastructureModule : Module
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>        
        /// <param key="configuration"></param> 
        public AddInfrastructureModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// Loading the lifetime Autofact of the services's Infrastructure.
        /// </summary>
        /// <param key="builder"></param>         
        protected override void Load(ContainerBuilder builder)
        {
            LegacyOptions sqlOptions = new LegacyOptions();
            _configuration.GetSection(LegacyOptions.Section).Bind(sqlOptions);
            // Registra el DbContext con Autofac
            
            _ = builder.RegisterType<HttpClient>().SingleInstance();

            _ = builder.RegisterType<GenericRestService>()
                .As<IGenericRestService>()
                .InstancePerLifetimeScope(); 
        }
    }
}
