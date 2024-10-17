using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace Integration.Orchestrator.Backend.Integration.Tests.Factory.DataInsertion
{
    public static class DataInserter<T> where T : class
    {
        public static void InsertEntity(IMongoDatabase database, string collectionName, string jsonData)
        {
            var collection = database.GetCollection<T>(collectionName);
            var documentList = JsonConvert.DeserializeObject<List<T>>(jsonData);

            collection.InsertMany(documentList);
        }

    }
}
