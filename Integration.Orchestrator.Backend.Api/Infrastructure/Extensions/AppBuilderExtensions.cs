using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Api.Infrastructure.Extensions
{
    /// <summary>
    /// Add to App Builder the Services instantiated
    /// </summary> 
    [ExcludeFromCodeCoverage]
    public static class AppBuilderExtensions
    {
        /// <summary>
        /// Loading the lifetime Autofact of the services's API.
        /// </summary>
        /// <param name="app"></param>         
        /// <param name="configuration"></param>        
        public static void AddAppConfigurationsInAssembly(this IApplicationBuilder app, IConfiguration configuration)
        {
            var customBuilders = typeof(Program).Assembly.DefinedTypes
                .Where(x => typeof(ICustomAppBuilder)
                    .IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract)
                .Select(Activator.CreateInstance)
                .Cast<ICustomAppBuilder>().ToList();

            customBuilders.ForEach(svc => svc.ConfigureApp(app, configuration));
        }
    }
}
