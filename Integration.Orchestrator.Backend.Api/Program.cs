using Autofac.Extensions.DependencyInjection;
using Autofac;
using Integration.Orchestrator.Backend.Api;
using Integration.Orchestrator.Backend.Api.Infrastructure.Extensions;
using Integration.Orchestrator.Backend.Application;
using Integration.Orchestrator.Backend.Domain;
using Integration.Orchestrator.Backend.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddServicesInAssembly(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Configuration.AddEnvironmentVariables();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
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
