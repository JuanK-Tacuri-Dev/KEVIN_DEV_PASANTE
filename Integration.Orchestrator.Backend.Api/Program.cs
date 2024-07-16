using Autofac.Extensions.DependencyInjection;
using Autofac;
using Integration.Orchestrator.Backend.Api;
using Integration.Orchestrator.Backend.Api.Infrastructure.Extensions;
using Integration.Orchestrator.Backend.Application;
using Integration.Orchestrator.Backend.Domain;
using Integration.Orchestrator.Backend.Infrastructure;
using Integration.Orchestrator.Backend.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Configure appsettings.json location
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("Config/appsettings.json", optional: false, reloadOnChange: true);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServicesInAssembly(builder.Configuration);
builder.Services.AddHttpContextAccessor();
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

app.AddAppConfigurationsInAssembly(builder.Configuration);
app.UseRouting();
app.MapControllers();
app.Run();
