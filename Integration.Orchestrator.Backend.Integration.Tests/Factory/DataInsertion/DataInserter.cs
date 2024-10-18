using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Helper;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace Integration.Orchestrator.Backend.Integration.Tests.Factory.DataInsertion
{
    public static class DataInserter<T> where T : class
    {
        public static void InsertStatus(IMongoDatabase database, string jsonData) 
        {
            var collection = database.GetCollection<StatusEntity>("Integration_Status");
            var documentList = JArray.Parse(jsonData)
                .Select(item => new StatusEntity 
                {
                    id = Guid.Parse(item["_id"].ToString()),
                    status_key = item["status_key"].ToString(),
                    status_text = item["status_text"].ToString(),
                    status_color = item["status_color"].ToString(),
                    status_background = item["status_background"].ToString(),
                    created_at = DateTime.ParseExact(item["created_at"].ToString(), ConfigurationSystem.DateTimeFormat, CultureInfo.InvariantCulture)
                    .ToLocalTime().ToString(ConfigurationSystem.DateTimeFormat),
                    updated_at = DateTime.ParseExact(item["updated_at"].ToString(), ConfigurationSystem.DateTimeFormat, CultureInfo.InvariantCulture)
                    .ToLocalTime().ToString(ConfigurationSystem.DateTimeFormat)
                })
                .ToList();
            collection.InsertManyAsync(documentList);
        }

        public static void InsertSynchronizationStates(IMongoDatabase database, string jsonData)
        {
            var collection = database.GetCollection<SynchronizationStatusEntity>("Integration_SynchronizationStates");
            var documentList = JArray.Parse(jsonData)
                .Select(item => new SynchronizationStatusEntity
                {
                    id = Guid.Parse(item["_id"].ToString()),
                    synchronization_status_key = item["synchronization_status_key"].ToString(),
                    synchronization_status_text = item["synchronization_status_text"].ToString(),
                    synchronization_status_color = item["synchronization_status_color"].ToString(),
                    synchronization_status_background = item["synchronization_status_background"].ToString(),
                    created_at = DateTime.ParseExact(item["created_at"].ToString(),ConfigurationSystem.DateTimeFormat,CultureInfo.InvariantCulture)
                    .ToLocalTime().ToString(ConfigurationSystem.DateTimeFormat),
                    updated_at = DateTime.ParseExact(item["updated_at"].ToString(),ConfigurationSystem.DateTimeFormat,CultureInfo.InvariantCulture)
                    .ToLocalTime().ToString(ConfigurationSystem.DateTimeFormat)
                })
                .ToList();
            collection.InsertManyAsync(documentList);
        }

        public static void InsertCatalog(IMongoDatabase database, string jsonData)
        {
            var collection = database.GetCollection<CatalogEntity>("Integration_Catalog");
            var documentList = JsonConvert.DeserializeObject<List<CatalogEntity>>(jsonData);

            collection.InsertManyAsync(documentList);
        }
        public static void InsertEntity(IMongoDatabase database, string collectionName, string jsonData)
        {
            var collection = database.GetCollection<T>(collectionName);
            var documentList = JsonConvert.DeserializeObject<List<T>>(jsonData);

            collection.InsertMany(documentList);
        }

    }
}
