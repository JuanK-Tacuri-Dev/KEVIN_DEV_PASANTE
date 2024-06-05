using Integration.Orchestrator.Backend.Api.Infrastructure.Extensions;
using Integration.Orchestrator.Backend.Application.Options;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
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
           // BsonDefaults.GuidRepresentation = GuidRepresentation.Standard;

            services.Configure<MongoOptions>(configuration.GetSection(MongoOptions.Section));

            var mongoSetting = services.BuildServiceProvider()
                .GetRequiredService<IOptions<MongoOptions>>().Value;

            var clientSettings = MongoClientSettings.FromUrl(new MongoUrl(mongoSetting.ConnectionString));

            var mongoClient = new MongoClient(clientSettings);
            services.AddSingleton(mongoClient);
            var database = mongoClient.GetDatabase(mongoSetting.DatabaseName);

             var synchronizationCollection = mongoSetting.Collections!.Synchronization;
            var synchronizationStatesCollection = mongoSetting.Collections!.SynchronizationStates;

             services.AddSingleton(s => database.GetCollection<SynchronizationEntity>(synchronizationCollection));
            services.AddSingleton(s => database.GetCollection<SynchronizationStatesEntity>(synchronizationStatesCollection));

            BsonSerializer.RegisterSerializer(new DecimalSerializer(BsonType.Decimal128));
        }
    }
}
