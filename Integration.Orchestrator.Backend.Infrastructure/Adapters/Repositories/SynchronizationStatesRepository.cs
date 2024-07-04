using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    public class SynchronizationStatesRepository(IMongoCollection<SynchronizationStatesEntity> collection) : ISynchronizationStatesRepository<SynchronizationStatesEntity>
    {
        private readonly IMongoCollection<SynchronizationStatesEntity> _collection = collection;

        public Task InsertAsync(SynchronizationStatesEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(SynchronizationStatesEntity entity)
        {
            var filter = Builders<SynchronizationStatesEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<SynchronizationStatesEntity>.Update
                .Set(m => m.name, entity.name)
                .Set(m => m.code, entity.code)
                .Set(m => m.color, entity.color)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(SynchronizationStatesEntity entity)
        {
            var filter = Builders<SynchronizationStatesEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<SynchronizationStatesEntity> GetByIdAsync(Expression<Func<SynchronizationStatesEntity, bool>> specification)
        {
            var filter = Builders<SynchronizationStatesEntity>.Filter.Where(specification);
            var synchronizationStatesEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return synchronizationStatesEntity;
        }

        public async Task<SynchronizationStatesEntity> GetByCodeAsync(Expression<Func<SynchronizationStatesEntity, bool>> specification)
        {
            var filter = Builders<SynchronizationStatesEntity>.Filter.Where(specification);
            var synchronizationStatesEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return synchronizationStatesEntity;
        }

        public async Task<IEnumerable<SynchronizationStatesEntity>> GetAllAsync(ISpecification<SynchronizationStatesEntity> specification)
        {
            var filter = Builders<SynchronizationStatesEntity>.Filter.Where(specification.Criteria);
            var synchronizationStatesEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<SynchronizationStatesEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<SynchronizationStatesEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return synchronizationStatesEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<SynchronizationStatesEntity> specification)
        {
            return await _collection.Find(specification.Criteria).CountDocumentsAsync();
        }
    }
}
