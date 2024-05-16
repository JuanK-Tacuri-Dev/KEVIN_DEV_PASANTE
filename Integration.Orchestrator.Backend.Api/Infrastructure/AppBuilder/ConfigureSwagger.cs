using Integration.Orchestrator.Backend.Api.Infrastructure.Extensions;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Api.Infrastructure.AppBuilder
{
    /// <summary>
    /// Configuration Swagger
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ConfigureSwagger : ICustomAppBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="configuration"></param>
        public void ConfigureApp(IApplicationBuilder app, IConfiguration configuration)
        {
            // Use swagger Doc
            app.UseSwagger();
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "GrupoKFC.BackOffice.Menus"); });
            app.ApplicationServices.SaveSwaggerJson();
        }
    }
}
