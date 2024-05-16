using Autofac;
using System.Diagnostics.CodeAnalysis;
using Module = Autofac.Module;

namespace Integration.Orchestrator.Backend.Application
{
    /// <summary>
    /// Register all infrastructure related objects
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AddApplicationModule : Module
    {
        /// <summary>
        /// Loading the lifetime Autofact of the services's Application.
        /// </summary>
        /// <param name="builder"></param>         
        protected override void Load(ContainerBuilder builder)
        {
            
        }
    }
}