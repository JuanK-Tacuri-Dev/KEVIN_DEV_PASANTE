using Integration.Orchestrator.Backend.Api.Infrastructure.Extensions;
using Integration.Orchestrator.Backend.Application.Options;
using Integration.Orchestrator.Backend.Domain.Entities;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Api.Infrastructure.ServiceRegistrations.Infrastructure
{
    /// <summary>
    /// Register Mongo Infrastructure to API
    /// </summary>   
    /// 
    [ExcludeFromCodeCoverage]
    public class RegisterMongo : IServiceRegistration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>  
        /// <param name="configuration"></param>  
        /// 
        public void RegisterAppServices(IServiceCollection services, IConfiguration configuration
        )
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
#pragma warning disable CS0618
            BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;
#pragma warning restore CS0618


            services.Configure<MongoOptions>(configuration.GetSection(MongoOptions.Section));

            var mongoSetting = services.BuildServiceProvider()
                .GetRequiredService<IOptions<MongoOptions>>().Value;

            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(mongoSetting.ConnectionString));

            var mongoClient = new MongoClient(clientSettings);
            services.AddSingleton(mongoClient);
            var database = mongoClient.GetDatabase(mongoSetting.DatabaseName);

            var synchronizationCollection = mongoSetting.Collections!.Synchronization;
            var synchronizationStatesCollection = mongoSetting.Collections!.SynchronizationStates;
            var connectionCollection = mongoSetting.Collections!.Connection;
            var integrationCollection = mongoSetting.Collections!?.Integration;
            var processCollection = mongoSetting.Collections!?.Process;

            services.AddSingleton(s => database.GetCollection<SynchronizationEntity>(synchronizationCollection));
            services.AddSingleton(s => database.GetCollection<SynchronizationStatesEntity>(synchronizationStatesCollection));
            services.AddSingleton(s => database.GetCollection<ConnectionEntity>(connectionCollection));
            services.AddSingleton(s => database.GetCollection<IntegrationEntity>(integrationCollection));
            services.AddSingleton(s => database.GetCollection<ProcessEntity>(processCollection));

            BsonClassMap.RegisterClassMap<Entity<Guid>>(
                map =>
                {
                    map.AutoMap();
                    map.MapProperty(x => x.id).SetSerializer(new GuidSerializer(BsonType.Binary));
                    map.MapIdMember(d => d.id).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                });
        }
    }
}
