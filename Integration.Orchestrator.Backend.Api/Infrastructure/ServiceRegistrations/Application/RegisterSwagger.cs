using Integration.Orchestrator.Backend.Api.Infrastructure.Extensions;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Integration.Orchestrator.Backend.Api.Infrastructure.ServiceRegistrations.Application
{
    /// <summary>
    /// Register App Service of Swagger.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RegisterSwagger : IServiceRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param key="services"></param>    
        /// <param key="configuration"></param>  
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(c =>
            {
                //c.SwaggerDoc("v1", new OpenApiInfo { Title = "API Integration.Orchestrator.Backend", Version = "v1" });

                //var xmlFile = $"{typeof(Program).GetTypeInfo().Assembly.GetName().Name}.xml";
                //var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //c.IncludeXmlComments(xmlPath);
            });
        }
    }
}
