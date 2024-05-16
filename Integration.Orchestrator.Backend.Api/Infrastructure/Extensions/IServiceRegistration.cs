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
        /// <param name="services"></param> 
        /// <param name="configuration"></param> 
        void RegisterAppServices(IServiceCollection services, IConfiguration configuration);
    }
}
