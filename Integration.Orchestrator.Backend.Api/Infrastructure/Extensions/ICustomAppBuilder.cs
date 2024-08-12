namespace Integration.Orchestrator.Backend.Api.Infrastructure.Extensions
{
    /// <summary>
    /// Interface to adapt the App Builder Factory.
    /// </summary>          
    public interface ICustomAppBuilder
    {
        /// <summary>
        /// Loading the lifetime Autofact of the services's API.
        /// </summary>
        /// <param key="app"></param>    
        /// <param key="configuration"></param>    
        void ConfigureApp(IApplicationBuilder app, IConfiguration configuration);
    }
}
