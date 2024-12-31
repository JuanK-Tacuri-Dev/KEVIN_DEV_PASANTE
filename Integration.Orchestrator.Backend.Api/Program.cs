using Autofac;
using Autofac.Extensions.DependencyInjection;
using Integration.Orchestrator.Backend.Api;
using Integration.Orchestrator.Backend.Api.Infrastructure.Extensions;
using Integration.Orchestrator.Backend.Application;
using Integration.Orchestrator.Backend.Application.Mappers;
using Integration.Orchestrator.Backend.Application.Mapping;
using Integration.Orchestrator.Backend.Domain;
using Integration.Orchestrator.Backend.Infrastructure;
using Integration.Orchestrator.Backend.Infrastructure.Extensions;
using System.Diagnostics.CodeAnalysis;


var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

switch (builder.Environment.EnvironmentName)
{
    case "Test":
        builder.Configuration.AddJsonFile("appsettings.TestIntegrations.json", optional: false, reloadOnChange: true);
        break;
    case "Development":
        builder.Configuration.AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);
        break;
    default:
        builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        break;
};
// Configurar CORS para permitir cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServicesInAssembly(builder.Configuration);
builder.Services.AddHttpContextAccessor();


builder.Services.AddAutoMapper(configuration =>
{
    configuration.AddProfile<MappingServerProfile>();
    configuration.AddProfile<MappingCatalogProfile>();
    configuration.AddProfile<MappingSynchronizationProfile>();

});
//builder.Configuration.AddEnvironmentVariables()

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
            .ConfigureServices((hostContext, services) =>
            {
                // Registrar servicios en IServiceCollection
                services.AddControllers();
                services.AddDomainServices();
            })
            .ConfigureContainer<ContainerBuilder>(builder =>
            {
                // Registrar servicios en Autofac ContainerBuilder
                builder.AddDomainServices();
            });

builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule(new AddApiModule(builder.Configuration));
    containerBuilder.RegisterModule(new AddInfrastructureModule(builder.Configuration));
    containerBuilder.RegisterModule(new AddDomainModule());
    containerBuilder.RegisterModule(new AddApplicationModule());
});


var app = builder.Build();
// Usar CORS
app.UseCors("AllowAllOrigins");
app.AddAppConfigurationsInAssembly(builder.Configuration);
app.UseRouting();
app.MapControllers();
app.Run();

[ExcludeFromCodeCoverage]
public partial class Program { }