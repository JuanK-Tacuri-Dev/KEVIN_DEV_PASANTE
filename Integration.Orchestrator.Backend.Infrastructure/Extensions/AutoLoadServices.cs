using Autofac;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Services;
using Integration.Orchestrator.Backend.Infrastructure.Adapters;
using Microsoft.Extensions.DependencyInjection;

namespace Integration.Orchestrator.Backend.Infrastructure.Extensions
{
    public static class AutoLoadServices
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            // Registrar servicios en IServiceCollection
            RegisterServices(services);
            return services;
        }

        public static ContainerBuilder AddDomainServices(this ContainerBuilder builder)
        {
            // Registrar servicios en ContainerBuilder de Autofac
            RegisterServices(builder);
            return builder;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            // generic repository
            //services.AddTransient(typeof(IRepository<>), typeof(GenericRepository<>));

            // unit of work
           // services.AddTransient<IUnitOfWork, UnitOfWork>();

            // Registrar servicios y repositorios
            RegisterDomainServices(services);
            RegisterInfrastructureServices(services);
            RegisterRepositories(services);
        }

        private static void RegisterServices(ContainerBuilder builder)
        {
            // Registrar servicios en Autofac ContainerBuilder
          //  builder.RegisterGeneric(typeof(GenericRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();
          //  builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerDependency();

            // Registrar servicios y repositorios
            RegisterDomainServices(builder);
            RegisterInfrastructureServices(builder);
            RegisterRepositories(builder);
        }

        private static void RegisterDomainServices(IServiceCollection services)
        {
            var _services = AppDomain.CurrentDomain.GetAssemblies()
                  .Where(assembly => assembly.FullName == null || assembly.FullName.Contains("Domain", StringComparison.InvariantCulture))
                  .SelectMany(s => s.GetTypes())
                  .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(DomainServiceAttribute)));

            foreach (var service in _services)
            {
                services.AddTransient(service);
            }
        }

        private static void RegisterDomainServices(ContainerBuilder builder)
        {
            var _services = AppDomain.CurrentDomain.GetAssemblies()
                  .Where(assembly => assembly.FullName == null || assembly.FullName.Contains("Domain", StringComparison.InvariantCulture))
                  .SelectMany(s => s.GetTypes())
                  .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(DomainServiceAttribute)));

            foreach (var service in _services)
            {
                var interfaces = service.GetInterfaces();
                foreach (var iface in interfaces)
                {
                    builder.RegisterType(service).As(iface).InstancePerDependency();
                }
            }
        }

        private static void RegisterInfrastructureServices(IServiceCollection services)
        {
            var _serviceInfrastructure = AppDomain.CurrentDomain.GetAssemblies()
                  .Where(assembly => assembly.FullName == null || assembly.FullName.Contains("Infrastructure", StringComparison.InvariantCulture))
                  .SelectMany(s => s.GetTypes())
                  .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(DomainServiceAttribute)));

            foreach (var service in _serviceInfrastructure)
            {
                Type iface = service.GetInterfaces().Single();
                services.AddTransient(iface, service);
            }
        }

        private static void RegisterInfrastructureServices(ContainerBuilder builder)
        {
            var _serviceInfrastructure = AppDomain.CurrentDomain.GetAssemblies()
                  .Where(assembly => assembly.FullName == null || assembly.FullName.Contains("Infrastructure", StringComparison.InvariantCulture))
                  .SelectMany(s => s.GetTypes())
                  .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(DomainServiceAttribute)));

            foreach (var service in _serviceInfrastructure)
            {
                var interfaces = service.GetInterfaces();
                foreach (var iface in interfaces)
                {
                    builder.RegisterType(service).As(iface).InstancePerDependency();
                }
            }
        }

        private static void RegisterRepositories(IServiceCollection services)
        {
            var _repositories = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName == null || assembly.FullName.Contains("Infrastructure", StringComparison.InvariantCulture))
                .SelectMany(s => s.GetTypes())
                .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(RepositoryAttribute)));

            foreach (var repo in _repositories)
            {
                Type iface = repo.GetInterfaces().Single();
                services.AddTransient(iface, repo);
            }
        }

        private static void RegisterRepositories(ContainerBuilder builder)
        {
            var _repositories = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => assembly.FullName == null || assembly.FullName.Contains("Infrastructure", StringComparison.InvariantCulture))
                .SelectMany(s => s.GetTypes())
                .Where(p => p.CustomAttributes.Any(x => x.AttributeType == typeof(RepositoryAttribute)));

            foreach (var repo in _repositories)
            {
                var interfaces = repo.GetInterfaces();
                foreach (var iface in interfaces)
                {
                    builder.RegisterType(repo).As(iface).InstancePerDependency();
                }
            }
        }
    }
}
