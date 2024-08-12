using Autofac;
using Integration.Orchestrator.Backend.Application.Options;
using Integration.Orchestrator.Backend.Domain.Entities.V2ToV1;
using Integration.Orchestrator.Backend.Domain.Ports;
using Integration.Orchestrator.Backend.Infrastructure.Adapters.Extractors.ExtractorSql.Contexts;
using Integration.Orchestrator.Backend.Infrastructure.Adapters.Loader;
using Integration.Orchestrator.Backend.Infrastructure.Adapters.Rest;
using Integration.Orchestrator.Backend.Infrastructure.Adapters.Transformators;
using Integration.Orchestrator.Backend.Infrastructure.DataAccess.Rest;
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

            //V1ToV2
            _ = builder.RegisterType<ExtractorV1Rest>()
                .As<IExtractor<string>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<LoaderToV2Rest>()
                .As<ILoader<string>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<TransformatorFromV1toV2Rest>()
                .As<ITransformator<string, string>>()
                .InstancePerLifetimeScope();

            //V2ToV1
            _ = builder.RegisterType<ExtractorV2Rest>()
                .As<IExtractor<TestEntityLegacy>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<LoaderToV1Rest>()
                .As<ILoader<TestEntity>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<TransformatorFromV2toV1Rest>()
                .As<ITransformator<TestEntityLegacy, TestEntity>>()
                .InstancePerLifetimeScope();
        }
    }
}
