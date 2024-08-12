using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using MongoDB.Driver;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [Repository]
    public class CodeConfiguratorRepository(
        IMongoCollection<CodeConfiguratorEntity> collection) 
        : ICodeConfiguratorRepository<CodeConfiguratorEntity>
    {
        private readonly IMongoCollection<CodeConfiguratorEntity> _collection = collection;

        public async Task<CodeConfiguratorEntity> GetByTypeAsync(int type)
        {
            return await _collection.Find(ms => ms.type == type).FirstOrDefaultAsync();
        }

        public Task<CodeConfiguratorEntity> IncrementModuleSequenceAsync(CodeConfiguratorEntity entity)
        {
            var filter = Builders<CodeConfiguratorEntity>.Filter.Eq(ms => ms.id, entity.id);
            var update = Builders<CodeConfiguratorEntity>.Update
                .Set(ms => ms.type, entity.type)
                .Set(ms => ms.value_text, entity.value_text)
                .Inc(ms => ms.value_number, 1);
            var options = new FindOneAndUpdateOptions<CodeConfiguratorEntity>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };
            return _collection.FindOneAndUpdateAsync(filter, update, options);;
        }
    }
}
