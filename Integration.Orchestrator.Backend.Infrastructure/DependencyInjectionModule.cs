using Autofac;
using Integration.Orchestrator.Backend.Application.Options;
using Integration.Orchestrator.Backend.Domain.Ports;
using Integration.Orchestrator.Backend.Infrastructure.DataAccess.Rest;
using Integration.Orchestrator.Backend.Infrastructure.DataAccess.Sql.Contexts;
using Integration.Orchestrator.Backend.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
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
        /// <param name="configuration"></param> 
        public AddInfrastructureModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// Loading the lifetime Autofact of the services's Infrastructure.
        /// </summary>
        /// <param name="builder"></param>         
        protected override void Load(ContainerBuilder builder)
        {
            LegacyOptions sqlOptions = new LegacyOptions();
            _configuration.GetSection(LegacyOptions.Section).Bind(sqlOptions);
            // Registra el DbContext con Autofac
            _ = builder.RegisterType<LegacyDbContext>()
                .As<DbContext>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<LegacyDbContext>()
               .As<DbContext>()
               .WithParameter("connectionStringSql", sqlOptions.ConnectionString)
               .SingleInstance();

            _ = builder.RegisterType<IntegrationV1Tov2Port>()
                .As<IIntegrationV1Tov2Port>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<HttpClient>().SingleInstance();

            _ = builder.RegisterType<GenericRestService>()
                .As<IGenericRestService>()
                .InstancePerLifetimeScope();
            


        }
    }
}
