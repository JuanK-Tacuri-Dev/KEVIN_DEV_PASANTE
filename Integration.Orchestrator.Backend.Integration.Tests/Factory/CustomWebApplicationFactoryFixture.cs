using Integration.Orchestrator.Backend.Application.Commons;
using Integration.Orchestrator.Backend.Application.Models.Administration.Adapter;
using Integration.Orchestrator.Backend.Application.Models.Administration.Connection;
using Integration.Orchestrator.Backend.Application.Models.Administration.Entities;
using Integration.Orchestrator.Backend.Application.Models.Administration.Integration;
using Integration.Orchestrator.Backend.Application.Models.Administration.Process;
using Integration.Orchestrator.Backend.Application.Models.Administration.Property;
using Integration.Orchestrator.Backend.Application.Models.Administration.Repository;
using Integration.Orchestrator.Backend.Application.Models.Administration.Server;
using Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Integration.Tests.Factory.Collections.NonMasters.Feed;
using Integration.Orchestrator.Backend.Integration.Tests.Factory.Collections.NonMasters.Feed.Cors.Model;
using Integration.Orchestrator.Backend.Integration.Tests.Factory.Collections.NonMasters.Feed.GetAllPaginated.Request;
using Integration.Orchestrator.Backend.Integration.Tests.Factory.DataInsertion;
using Integration.Orchestrator.Backend.Integration.Tests.Factory.Helpers;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

namespace Integration.Orchestrator.Backend.Integration.Tests.Factory
{
    public class CustomWebApplicationFactoryFixture : IDisposable
    {
        // Agrupación de rutas de archivos en una clase de constantes
        public static class JsonPaths
        {
            public const string NonMasterCollections = "Factory/Collections/NonMasters/non_master_collections.json";
            public const string MasterCollections = "Factory/Collections/Masters/master_collections.json";
            public const string CorsSettings = "Factory/Collections/NonMasters/Feed/Cors/Json/CorsSettings.json";
            public const string GetAllPaginated = "Factory/Collections/NonMasters/Feed/GetAllPaginated/Json/ValidGetAllPaginated.json";
            public const string ServerBasicInfo = "Factory/Collections/NonMasters/Feed/Server/Json/ValidServerBasicInfoRequest.json";
            public const string AdapterBasicInfo = "Factory/Collections/NonMasters/Feed/Adapter/Json/ValidAdapterBasicInfoRequest.json";
            public const string RepositoryBasicInfo = "Factory/Collections/NonMasters/Feed/Repository/Json/ValidRepositoryBasicInfoRequest.json";
            public const string ConnectionBasicInfo = "Factory/Collections/NonMasters/Feed/Connection/Json/ValidConnectionBasicInfoRequest.json";
            public const string EntityBasicInfo = "Factory/Collections/NonMasters/Feed/Entity/Json/ValidEntityBasicInfoRequest.json";
            public const string PropertyBasicInfo = "Factory/Collections/NonMasters/Feed/Property/Json/ValidPropertyBasicInfoRequest.json";
            public const string ProcessBasicInfo = "Factory/Collections/NonMasters/Feed/Process/Json/ValidProcessBasicInfoRequest.json";
            public const string IntegrationBasicInfo = "Factory/Collections/NonMasters/Feed/Integration/Json/ValidIntegrationBasicInfoRequest.json";
            public const string SynchronizationBasicInfo = "Factory/Collections/NonMasters/Feed/Synchronization/Json/ValidSynchronizationBasicInfoRequest.json";
            public const string FeedDirectory = "Factory/Collections/Masters/Feed";
        }

        public CustomWebApplicationFactory<Program> Factory { get; private set; }
        public IMongoDatabase? Database => Factory?.Database;
        public CorsSettings CorsSettings { get; private set; }
        public PaginatedDefinition ValidGetAllPaginated { get; private set; }
        public ServerCreateRequest ValidServerCreateRequest { get; private set; }
        public AdapterCreateRequest ValidAdapterCreateRequest { get; private set; }
        public RepositoryCreateRequest ValidRepositoryCreateRequest { get; private set; }
        public ConnectionCreateRequest ValidConnectionCreateRequest { get; private set; }
        public EntitiesCreateRequest ValidEntityCreateRequest { get; private set; }
        public PropertyCreateRequest ValidPropertyCreateRequest { get; private set; }
        public ProcessCreateRequest ValidProcessCreateRequest { get; private set; }
        public IntegrationCreateRequest ValidIntegrationCreateRequest { get; private set; }
        public SynchronizationCreateRequest ValidSynchronizationCreateRequest { get; private set; }

        private List<string> _nonMasterCollections;
        private List<string> _masterCollections;
        private HttpClient _client;

        #pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public CustomWebApplicationFactoryFixture()
        {
            Factory = new CustomWebApplicationFactory<Program>();
            _client = Factory.CreateClient();
            if (Factory.Database == null)
            {
                throw new Exception("Factory.Database is not initialized.");
            }

            _nonMasterCollections = LoadCollectionNames(JsonPaths.NonMasterCollections);
            _masterCollections = LoadCollectionNames(JsonPaths.MasterCollections);

            InitializeCollections(_nonMasterCollections);
            InitializeCollections(_masterCollections);
            LoadDataToMasterCollections(JsonPaths.FeedDirectory);
            InitializeObjects();
        }

        public HttpClient GetHttpClient() => _client ??= Factory.CreateClient();

        private List<string> LoadCollectionNames(string jsonFilePath)
        {
            if (!File.Exists(jsonFilePath))
            {
                throw new FileNotFoundException($"The file {jsonFilePath} was not found.");
            }

            string jsonString = File.ReadAllText(jsonFilePath);

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };

            var collections = JsonSerializer.Deserialize<CollectionList>(jsonString, options);
            return collections?.Collections ?? new List<string>();
        }

        private void InitializeCollections(List<string> collections)
        {
            var database = Factory.Database;
            if (database == null)
            {
                throw new Exception("Database is not initialized.");
            }

            var existingCollections = database.ListCollectionNames().ToList();

            foreach (var collectionName in collections)
            {
                if (existingCollections.Contains(collectionName))
                {
                    var collection = database.GetCollection<BsonDocument>(collectionName);
                    collection.DeleteMany(new BsonDocument());
                }
                else
                {
                    database.CreateCollection(collectionName);
                }
            }
        }

        private void LoadDataToMasterCollections(string feedDirectory)
        {
            var files = Directory.GetFiles(feedDirectory, "*.json");

            foreach (var file in files)
            {
                var collectionName = Path.GetFileNameWithoutExtension(file).Replace('.', '_');
                var jsonString = File.ReadAllText(file);
                InsertDataIntoCollection(collectionName, jsonString);
            }
        }

        private void InsertDataIntoCollection(string collectionName, string jsonString)
        {
            try
            {
                var database = Factory.Database;
                if (database == null) return;

                switch (collectionName)
                {
                    case "Integration_Status":
                        if (database != null) DataInserter<StatusEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    case "Integration_SynchronizationStates":
                        if (database != null) DataInserter<SynchronizationStatusEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    case "Integration_Catalog":
                        if (database != null) DataInserter<CatalogEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    case "Integration_Server":
                        if (database != null) DataInserter<ServerEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    case "Integration_Adapter":
                        if (database != null) DataInserter<AdapterEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    case "Integration_Repository":
                        if (database != null) DataInserter<RepositoryEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    case "Integration_Connection":
                        if (database != null) DataInserter<ConnectionEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    case "Integration_Entity":
                        if (database != null) DataInserter<EntitiesEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    case "Integration_Property":
                        if (database != null) DataInserter<PropertyEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    case "Integration_Process":
                        if (database != null) DataInserter<ProcessEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    case "Integration_Integration":
                        if (database != null) DataInserter<IntegrationEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    case "Integration_Synchronization":
                        if (database != null) DataInserter<SynchronizationEntity>.InsertEntity(database, collectionName, jsonString);
                        break;
                    default:
                        throw new Exception($"No method found to handle collection {collectionName}");
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while loading data to master collections: {ex.Message}", ex);
            }
        }

        private void InitializeObjects()
        {
            // Aquí se deben inicializar más objetos según sea necesario
            CorsSettings = JsonReader<CorsSettings>.ReadBasicInfoRequest(JsonPaths.CorsSettings);
            ValidGetAllPaginated = GetAllPaginatedJsonReader.ReadValidGetAllPaginated(JsonPaths.GetAllPaginated);
            ValidServerCreateRequest = JsonReader<ServerCreateRequest>.ReadBasicInfoRequest(JsonPaths.ServerBasicInfo);
            ValidAdapterCreateRequest = JsonReader<AdapterCreateRequest>.ReadBasicInfoRequest(JsonPaths.AdapterBasicInfo);
            ValidRepositoryCreateRequest = JsonReader<RepositoryCreateRequest>.ReadBasicInfoRequest(JsonPaths.RepositoryBasicInfo);
            ValidConnectionCreateRequest = JsonReader<ConnectionCreateRequest>.ReadBasicInfoRequest(JsonPaths.ConnectionBasicInfo);
            ValidEntityCreateRequest = JsonReader<EntitiesCreateRequest>.ReadBasicInfoRequest(JsonPaths.EntityBasicInfo);
            ValidPropertyCreateRequest = JsonReader<PropertyCreateRequest>.ReadBasicInfoRequest(JsonPaths.PropertyBasicInfo);
            ValidProcessCreateRequest = JsonReader<ProcessCreateRequest>.ReadBasicInfoRequest(JsonPaths.ProcessBasicInfo);
            ValidIntegrationCreateRequest = JsonReader<IntegrationCreateRequest>.ReadBasicInfoRequest(JsonPaths.IntegrationBasicInfo);
            ValidSynchronizationCreateRequest = JsonReader<SynchronizationCreateRequest>.ReadBasicInfoRequest(JsonPaths.SynchronizationBasicInfo);
        }

        private void CleanUpCollections(List<string> collections)
        {
            var database = Factory.Database;
            if (database == null)
            {
                throw new Exception("Database is not initialized.");
            }

            foreach (var collectionName in collections)
            {
                var collection = database.GetCollection<BsonDocument>(collectionName);
                collection.DeleteMany(Builders<BsonDocument>.Filter.Empty);
            }
        }

        public void Dispose()
        {
            CleanUpCollections(_nonMasterCollections);
            _client.Dispose();
            Factory.Dispose();
        }

        public void DisposeMethod(List<string> collections)
        {
            CleanUpCollections(collections);
        }
    }

    [CollectionDefinition("CustomWebApplicationFactory collection", DisableParallelization = true)]
    public class CustomWebApplicationFactoryCollection : ICollectionFixture<CustomWebApplicationFactoryFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
