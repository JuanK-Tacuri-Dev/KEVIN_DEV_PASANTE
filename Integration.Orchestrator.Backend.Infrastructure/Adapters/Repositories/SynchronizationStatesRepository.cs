using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [ExcludeFromCodeCoverage]
    [Repository]
    public class SynchronizationStatesRepository(IMongoCollection<SynchronizationStatusEntity> collection)
        : ISynchronizationStatesRepository<SynchronizationStatusEntity>
    {
        private readonly IMongoCollection<SynchronizationStatusEntity> _collection = collection;

        public Task InsertAsync(SynchronizationStatusEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(SynchronizationStatusEntity entity)
        {
            var filter = Builders<SynchronizationStatusEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<SynchronizationStatusEntity>.Update
                .Set(m => m.synchronization_status_key, entity.synchronization_status_key)
                .Set(m => m.synchronization_status_text, entity.synchronization_status_text)
                .Set(m => m.synchronization_status_color, entity.synchronization_status_color)
                .Set(m => m.synchronization_status_background, entity.synchronization_status_background)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(SynchronizationStatusEntity entity)
        {
            var filter = Builders<SynchronizationStatusEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<SynchronizationStatusEntity> GetByIdAsync(Expression<Func<SynchronizationStatusEntity, bool>> specification)
        {
            var filter = Builders<SynchronizationStatusEntity>.Filter.Where(specification);
            var synchronizationStatesEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return synchronizationStatesEntity;
        }

        public async Task<IEnumerable<SynchronizationStatusEntity>> GetByKeysAsync(Expression<Func<SynchronizationStatusEntity, bool>> specification)
        {
            var filter = Builders<SynchronizationStatusEntity>.Filter.Where(specification);
            var synchronizationStatesEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return synchronizationStatesEntity;
        }
        public async Task<SynchronizationStatusEntity> GetByKeyAsync(Expression<Func<SynchronizationStatusEntity, bool>> specification)
        {
            var filter = Builders<SynchronizationStatusEntity>.Filter.Where(specification);
            var synchronizationStatesEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return synchronizationStatesEntity;
        }

        public async Task<IEnumerable<SynchronizationStatusEntity>> GetAllAsync(ISpecification<SynchronizationStatusEntity> specification)
        {
            var filter = Builders<SynchronizationStatusEntity>.Filter.Where(specification.Criteria);

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<SynchronizationStatusEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<SynchronizationStatusEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<SynchronizationStatusEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }
    }
}
