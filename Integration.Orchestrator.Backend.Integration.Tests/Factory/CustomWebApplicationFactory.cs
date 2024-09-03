using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;

namespace Integration.Orchestrator.Backend.Integration.Tests.Factory
{
    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
    {
        public IMongoDatabase? Database { get; private set; }
        private readonly ILogger<CustomWebApplicationFactory<TStartup>> _logger;


        public CustomWebApplicationFactory()
        {
            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Debug);
            });
            _logger = loggerFactory.CreateLogger<CustomWebApplicationFactory<TStartup>>();
        }

        protected override IHost CreateHost(IHostBuilder builder)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.TestIntegrations.json")
                    .Build();

                var mongoConnectionString = configuration.GetSection("MongoDB:ConnectionString").Value;
                var mongoDatabaseName = configuration.GetSection("MongoDB:DatabaseName").Value;

                _logger.LogInformation("MongoDB Connection String: {ConnectionString}", mongoConnectionString);
                _logger.LogInformation("MongoDB Database Name: {DatabaseName}", mongoDatabaseName);

                if (string.IsNullOrEmpty(mongoConnectionString) || string.IsNullOrEmpty(mongoDatabaseName))
                {
                    throw new Exception("MongoDB connection string or database name is missing in configuration.");
                }

                var mongoClientSettings = MongoClientSettings.FromUrl(new MongoUrl(mongoConnectionString));
                mongoClientSettings.ClusterConfigurator = cb =>
                {
                    cb.Subscribe<CommandStartedEvent>(e =>
                    {
                        _logger.LogDebug("{CommandName} - {Command}", e.CommandName, e.Command.ToJson());
                    });
                };

                var client = new MongoClient(mongoClientSettings);
                Database = client.GetDatabase(mongoDatabaseName);

                if (Database == null)
                {
                    throw new Exception("Failed to initialize MongoDB database.");
                }

                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(Database);
                    services.AddSingleton<IMongoClient>(client);
                });

                builder.UseEnvironment("Test");
                return base.CreateHost(builder);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initializing the MongoDB database.");
                throw;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }
}
