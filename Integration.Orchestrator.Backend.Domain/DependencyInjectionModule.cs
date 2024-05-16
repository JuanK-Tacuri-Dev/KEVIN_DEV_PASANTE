using Autofac;
using Integration.Orchestrator.Backend.Domain.Entities;
using Integration.Orchestrator.Backend.Domain.Services;
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
        }
    }
}
