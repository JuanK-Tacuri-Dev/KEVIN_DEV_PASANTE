using Integration.Orchestrator.Backend.Integration.Tests.Factory.Collections.NonMasters.Feed.Cross.Model;
using Integration.Orchestrator.Backend.Integration.Tests.Factory.Collections.NonMasters.Feed.Cross.Request;
using Integration.Orchestrator.Backend.Integration.Tests.Factory.Helpers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

namespace Integration.Orchestrator.Backend.Integration.Tests.Factory
{
    public class CustomWebApplicationFactoryFixture : IDisposable
    {
        public const string NonMasterCollectionsPath = "Factory/Collections/NonMasters/non_master_collections.json";
        public const string MasterCollectionsPath = "Factory/Collections/Masters/master_collections.json";
        public const string CrossHeadersJsonPath = "Factory/Collections/NonMasters/Feed/Cross/Json/CrossHeadersSettings.json";
        public const string FeedDirectory = "Factory/Collections/Masters/Feed";

        public CustomWebApplicationFactory<Program> Factory { get; private set; }
        private HttpClient _client;
        public IMongoDatabase Database => Factory.Database;
        public CrossHeadersSettings CrossHeadersSettings { get; private set; }

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
                        //case "Franchise":
                        //    if (database != null) DataInserter.InsertFranchiseData(database, jsonString);
                        //    break;
                        //case "InternalClients_Restaurant":
                        //    if (database != null) DataInserter.InsertRestaurantsData(database, jsonString);
                        //    break;
                        //case "InternalClients_Users":
                        //    if (database != null) DataInserter.InsertUserData(database, jsonString);
                        //    break;
                        //case "InternalClients_Franchise_Configurations":
                        //    if (database != null) DataInserter.InsertConfigurationFranchiseData(database, jsonString);
                        //    break;
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
            CrossHeadersSettings = CrossHeadersJsonReader.ReadValidCrossHeadersSettings(CrossHeadersJsonPath);
            // Aquí se deben inicializar más objetos según sea necesario
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
