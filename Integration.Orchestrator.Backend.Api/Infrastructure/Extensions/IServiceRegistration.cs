namespace Integration.Orchestrator.Backend.Api.Infrastructure.Extensions
{
    /// <summary>
    /// Interface to allow the Register Services to Api Pipeline
    /// </summary>     
    public interface IServiceRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param key="services"></param> 
        /// <param key="configuration"></param> 
        void RegisterAppServices(IServiceCollection services, IConfiguration configuration);
    }
}
