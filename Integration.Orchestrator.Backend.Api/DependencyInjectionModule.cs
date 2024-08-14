using Autofac;
using Integration.Orchestrator.Backend.Api.Filter;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Api
{
    /// <summary>
    /// Register all infrastructure related objects
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AddApiModule : Module
    {

        private readonly IConfiguration _configuration;

        /// <summary>
        /// 
        /// </summary>        
        /// <param key="configuration"></param> 
        public AddApiModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Loading the lifetime Autofact of the services's API.
        /// </summary>
        /// <param key="builder"></param>         
        protected override void Load(ContainerBuilder builder)
        {
            _ = builder.RegisterInstance(_configuration).As<IConfiguration>();

            _ = builder.RegisterType<ErrorHandlingRest>().AsSelf().SingleInstance();


        }
    }
}
