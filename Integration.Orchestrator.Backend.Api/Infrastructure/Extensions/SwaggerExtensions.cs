using Microsoft.OpenApi.Extensions;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Api.Infrastructure.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerExtensions
    {
        public static void SaveSwaggerJson(this IServiceProvider provider)
        {
            ISwaggerProvider sw = provider.GetRequiredService<ISwaggerProvider>();
            OpenApiDocument doc = sw.GetSwagger("v1", null, "/");
            string swaggerFile = doc.SerializeAsJson(Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0);
            string filePath = Path.Combine("..", "Deployment", "Swagger", "BackOffice_Swaggerfile.json");
            Directory.CreateDirectory(Path.GetDirectoryName(filePath));
            File.WriteAllText(filePath, swaggerFile);
        }
    }
}
