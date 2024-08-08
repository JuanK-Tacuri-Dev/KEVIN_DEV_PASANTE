using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using MongoDB.Driver;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [Repository]
    public class ModuleSequenceRepository(
        IMongoCollection<ModuleSequenceEntity> collection) 
        : IModuleSequenceRepository<ModuleSequenceEntity>
    {
        private readonly IMongoCollection<ModuleSequenceEntity> _collection = collection;

        public async Task<ModuleSequenceEntity> GetByModuleNameAsync(string moduleName)
        {
            return await _collection.Find(ms => ms.id == moduleName).FirstOrDefaultAsync();
        }

        public Task<ModuleSequenceEntity> IncrementModuleSequenceAsync(string moduleName)
        {
            var filter = Builders<ModuleSequenceEntity>.Filter.Eq(ms => ms.id, moduleName);
            var update = Builders<ModuleSequenceEntity>.Update
                .Inc(ms => ms.last_sequence, 1)
                .Set(ms => ms.id, moduleName);
            var options = new FindOneAndUpdateOptions<ModuleSequenceEntity>
            {
                IsUpsert = true,
                ReturnDocument = ReturnDocument.After
            };
            return _collection.FindOneAndUpdateAsync(filter, update, options);;
        }

        public async Task<ModuleSequenceEntity> UpdateModuleSequenceAsync(string moduleName, int newSequence)
        {
            var filter = Builders<ModuleSequenceEntity>.Filter.Eq(ms => ms.id, moduleName);
            var update = Builders<ModuleSequenceEntity>.Update.Set(ms => ms.last_sequence, newSequence);
            return await _collection
                .FindOneAndUpdateAsync(filter, update, new FindOneAndUpdateOptions<ModuleSequenceEntity>
                {
                    ReturnDocument = ReturnDocument.After
                });
        }
    }
}
