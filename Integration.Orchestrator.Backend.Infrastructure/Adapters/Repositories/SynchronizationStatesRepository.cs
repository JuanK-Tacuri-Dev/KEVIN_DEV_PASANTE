using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Ports.Administrations.Synchronization;
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

        public async Task<SynchronizationStatesEntity> GetByIdAsync(Guid id)
        {
            var specification = (Expression<Func<SynchronizationStatesEntity, bool>>)(x => x.id == id);
            var filter = Builders<SynchronizationStatesEntity>.Filter.Where(specification);
            var synchronizationEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return synchronizationEntity;
        }

        public async Task<IEnumerable<SynchronizationStatesEntity>> GetAllAsync(ISpecification<SynchronizationStatesEntity> specification)
        {
            var filter = Builders<SynchronizationStatesEntity>.Filter.Where(specification.Criteria);
            var synchronizationEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<SynchronizationStatesEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<SynchronizationStatesEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return synchronizationEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<SynchronizationStatesEntity> specification)
        {
            return await _collection.Find(specification.Criteria).CountDocumentsAsync();
        }
    }
}
