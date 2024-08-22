using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [Repository]
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
                .Set(m => m.synchronization_name, entity.synchronization_name)
                .Set(m => m.franchise_id, entity.franchise_id)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.synchronization_observations, entity.synchronization_observations)
                .Set(m => m.user_id, entity.user_id)
                .Set(m => m.synchronization_hour_to_execute, entity.synchronization_hour_to_execute)
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

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<SynchronizationEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<SynchronizationEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<SynchronizationEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

    }
}
