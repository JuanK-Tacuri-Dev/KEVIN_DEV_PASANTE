using Autofac;
using Integration.Orchestrator.Backend.Domain.Entities;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.V2ToV1;
using Integration.Orchestrator.Backend.Domain.Services;
using Integration.Orchestrator.Backend.Domain.Services.Administration;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Domain
{
    /// <summary>
    /// Register all infrastructure related objects
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AddDomainModule : Module
    {
        /// <summary>
        /// Loading the lifetime Autofact of the services's Domain.
        /// </summary>
        /// <param name="builder"></param>         
        protected override void Load(ContainerBuilder builder)
        {
            _ = builder.RegisterType<IntregrationV1ToV2Service>()
                .As<IIntregrationV1ToV2Service>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<IntregrationV2ToV1Service>()
                .As<IIntregrationV2ToV1Service>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<SynchronizationService>()
                .As<ISynchronizationService<SynchronizationEntity>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<SynchronizationStatesService>()
                .As<ISynchronizationStatesService<SynchronizationStatesEntity>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<ConnectionService>()
                .As<IConnectionService<ConnectionEntity>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<IntegrationService>()
                .As<IIntegrationService<IntegrationEntity>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<ProcessService>()
                .As<IProcessService<ProcessEntity>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<StatusService>()
                .As<IStatusService<StatusEntity>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<EntitiesService>()
                .As<IEntitiesService<EntitiesEntity>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<PropertyService>()
                .As<IPropertyService<PropertyEntity>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<ValueService>()
                .As<IValueService<ValueEntity>>()
                .InstancePerLifetimeScope();

            _ = builder.RegisterType<OperatorService>()
                .As<IOperatorService<OperatorEntity>>()
                .InstancePerLifetimeScope();
        }
    }
}
