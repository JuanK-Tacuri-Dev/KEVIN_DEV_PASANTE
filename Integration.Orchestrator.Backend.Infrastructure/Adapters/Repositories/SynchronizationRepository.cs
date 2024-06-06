using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Ports.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    public class SynchronizationRepository(IMongoCollection<SynchronizationEntity> collection) : ISynchronizationRepository<SynchronizationEntity>
    {
        private readonly IMongoCollection<SynchronizationEntity> _collection = collection;
        public Task InsertAsync(SynchronizationEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(SynchronizationEntity entity)
        {
            var filter = Builders<SynchronizationEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<SynchronizationEntity>.Update
                .Set(m => m.name, entity.name)
                .Set(m => m.franchise_id, entity.franchise_id)
                .Set(m => m.status, entity.status)
                .Set(m => m.observations, entity.observations)
                .Set(m => m.user_id, entity.user_id)
                .Set(m => m.hour_to_execute, entity.hour_to_execute)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(SynchronizationEntity entity)
        {
            var filter = Builders<SynchronizationEntity>.Filter.Eq("_id", entity.id);
            return _collection.DeleteOneAsync(filter);
        }

        public async Task<SynchronizationEntity> GetByIdAsync(Expression<Func<SynchronizationEntity, bool>> specification)
        {
            var filter = Builders<SynchronizationEntity>.Filter.Where(specification);
            var synchronizationEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return synchronizationEntity;
        }

        public async Task<IEnumerable<SynchronizationEntity>> GetByFranchiseIdAsync(Expression<Func<SynchronizationEntity, bool>> specification)
        {
            var filter = Builders<SynchronizationEntity>.Filter.Where(specification);
            var synchronizationEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return synchronizationEntity;
        }

        public async Task<IEnumerable<SynchronizationEntity>> GetAllAsync(ISpecification<SynchronizationEntity> specification)
        {
            var filter = Builders<SynchronizationEntity>.Filter.Where(specification.Criteria);
            var synchronizationEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<SynchronizationEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<SynchronizationEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return synchronizationEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<SynchronizationEntity> specification)
        {
            return await _collection.Find(specification.Criteria).CountDocumentsAsync();
        }

    }
}
