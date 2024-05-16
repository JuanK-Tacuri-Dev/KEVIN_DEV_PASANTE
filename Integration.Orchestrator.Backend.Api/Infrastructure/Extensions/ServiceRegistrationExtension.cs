using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Api.Infrastructure.Extensions
{
    /// <summary>
    /// Class to register all Services in the API
    /// </summary>    
    [ExcludeFromCodeCoverage]
    public static class ServiceRegistrationExtension
    {
        /// <summary>
        /// 
        /// </summary>    
        public static void AddServicesInAssembly(this IServiceCollection services, IConfiguration configuration)
        {
            var appServices = typeof(Program).Assembly.DefinedTypes
                .Where(x => typeof(IServiceRegistration)
                    .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<IServiceRegistration>()
                .ToList();

            appServices.ForEach(svc => svc.RegisterAppServices(services, configuration));
        }
    }
}
