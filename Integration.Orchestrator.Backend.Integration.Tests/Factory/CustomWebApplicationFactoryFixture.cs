﻿using Integration.Orchestrator.Backend.Application.Commons;
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
        public const string NonMasterCollectionsPath = "Factory/Collections/NonMasters/non_master_collections.json";
        public const string MasterCollectionsPath = "Factory/Collections/Masters/master_collections.json";
        public const string CorsJsonPath = "Factory/Collections/NonMasters/Feed/Cors/Json/CorsSettings.json";
        public const string GetAllPaginatedJsonPath = "Factory/Collections/NonMasters/Feed/GetAllPaginated/Json/ValidGetAllPaginated.json";
        public const string ServerJsonPath = "Factory/Collections/NonMasters/Feed/Server/Json/ValidServerBasicInfoRequest.json";
        public const string AdapterJsonPath = "Factory/Collections/NonMasters/Feed/Adapter/Json/ValidAdapterBasicInfoRequest.json";
        public const string RepositoryJsonPath = "Factory/Collections/NonMasters/Feed/Repository/Json/ValidRepositoryBasicInfoRequest.json";
        public const string ConnectionJsonPath = "Factory/Collections/NonMasters/Feed/Connection/Json/ValidConnectionBasicInfoRequest.json";
        public const string EntityJsonPath = "Factory/Collections/NonMasters/Feed/Entity/Json/ValidEntityBasicInfoRequest.json";
        public const string PropertyJsonPath = "Factory/Collections/NonMasters/Feed/Property/Json/ValidPropertyBasicInfoRequest.json";
        public const string ProcessJsonPath = "Factory/Collections/NonMasters/Feed/Process/Json/ValidProcessBasicInfoRequest.json";
        public const string IntegrationJsonPath = "Factory/Collections/NonMasters/Feed/Integration/Json/ValidIntegrationBasicInfoRequest.json";
        public const string SynchronizationJsonPath = "Factory/Collections/NonMasters/Feed/Synchronization/Json/ValidSynchronizationBasicInfoRequest.json";
        public const string FeedDirectory = "Factory/Collections/Masters/Feed";

        public CustomWebApplicationFactory<Program> Factory { get; private set; }
        private HttpClient _client;
        public IMongoDatabase Database => Factory.Database;
        public CorsSettings corsSettings {  get; private set; }
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

        private List<string> nonMasterCollections;
        private List<string> masterCollections;

        public CustomWebApplicationFactoryFixture()
        {
            Factory = new CustomWebApplicationFactory<Program>();
            _client = Factory.CreateClient();
            if (Factory.Database == null)
            {
                throw new Exception("Factory.Database is not initialized.");
            }

            nonMasterCollections = LoadCollectionNames(NonMasterCollectionsPath);
            masterCollections = LoadCollectionNames(MasterCollectionsPath);

            InitializeNonMasterCollections();
            InitializeMasterCollectionsWithData();
            InitializeObjects();
        }

        public HttpClient GetHttpClient()
        {
            return _client ??= Factory.CreateClient();
        }

        private void InitializeNonMasterCollections()
        {
            InitializeCollections(nonMasterCollections);
        }

        private void InitializeMasterCollectionsWithData()
        {
            InitializeCollections(masterCollections);
            LoadDataToMasterCollections(FeedDirectory);
        }

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
            if (collections == null || collections?.Collections == null || collections.Collections.Count == 0)
            {
                //throw new Exception("No collections found in the specified JSON file.");
                return new List<string>();
            }

            return collections.Collections;
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
            try
            {
                var files = Directory.GetFiles(feedDirectory, "*.json");
                var database = Factory.Database;

                foreach (var file in files)
                {
                    string jsonString = File.ReadAllText(file);
                    var collectionName = Path.GetFileNameWithoutExtension(file).Replace('.', '_');

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
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while loading data to master collections: {ex.Message}", ex);
            }
        }

        private void InitializeObjects()
        {
            // Aquí se deben inicializar más objetos según sea necesario
            corsSettings = JsonReader<CorsSettings>.ReadBasicInfoRequest(CorsJsonPath);
            ValidGetAllPaginated = GetAllPaginatedJsonReader.ReadValidGetAllPaginated(GetAllPaginatedJsonPath);
            ValidServerCreateRequest = JsonReader<ServerCreateRequest>.ReadBasicInfoRequest(ServerJsonPath);
            ValidAdapterCreateRequest = JsonReader<AdapterCreateRequest>.ReadBasicInfoRequest(AdapterJsonPath);
            ValidRepositoryCreateRequest = JsonReader<RepositoryCreateRequest>.ReadBasicInfoRequest(RepositoryJsonPath);
            ValidConnectionCreateRequest = JsonReader<ConnectionCreateRequest>.ReadBasicInfoRequest(ConnectionJsonPath);
            ValidEntityCreateRequest = JsonReader<EntitiesCreateRequest>.ReadBasicInfoRequest(EntityJsonPath);
            ValidPropertyCreateRequest = JsonReader<PropertyCreateRequest>.ReadBasicInfoRequest(PropertyJsonPath);
            ValidProcessCreateRequest = JsonReader<ProcessCreateRequest>.ReadBasicInfoRequest(ProcessJsonPath);
            ValidIntegrationCreateRequest = JsonReader<IntegrationCreateRequest>.ReadBasicInfoRequest(IntegrationJsonPath);
            ValidSynchronizationCreateRequest = JsonReader<SynchronizationCreateRequest>.ReadBasicInfoRequest(SynchronizationJsonPath);
        }

        private void CleanUpNonMasterCollections(List<string> collections)
        {
            var database = Factory.Database;
            if (database == null)
            {
                throw new Exception("Database is not initialized.");
            }

            var filter = Builders<BsonDocument>.Filter.Empty;

            foreach (var collectionName in collections)
            {
                var collection = database.GetCollection<BsonDocument>(collectionName);
                collection.DeleteMany(filter);
            }
        }

        public void Dispose()
        {
            CleanUpNonMasterCollections(nonMasterCollections);
            _client.Dispose();
            Factory.Dispose();
        }

        public void DisposeMethod(List<string> collections)
        {
            CleanUpNonMasterCollections(collections);
        }
    }

    [CollectionDefinition("CustomWebApplicationFactory collection")]
    public class CustomWebApplicationFactoryCollection : ICollectionFixture<CustomWebApplicationFactoryFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
