using Autofac;
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
        /// <param key="builder"></param>         
        protected override void Load(ContainerBuilder builder)
        {
        }
    }
}
