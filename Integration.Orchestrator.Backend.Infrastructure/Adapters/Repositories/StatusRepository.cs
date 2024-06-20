using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    public class StatusRepository(IMongoCollection<StatusEntity> collection) : IStatusRepository<StatusEntity>
    {
        private readonly IMongoCollection<StatusEntity> _collection = collection;
        public Task InsertAsync(StatusEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public async Task<IEnumerable<StatusEntity>> GetAllAsync(ISpecification<StatusEntity> specification)
        {
            var filter = Builders<StatusEntity>.Filter.Where(specification.Criteria);
            var synchronizationEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<StatusEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<StatusEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return synchronizationEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<StatusEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .Limit(specification.Limit)
                .Skip(specification.Skip).CountDocumentsAsync();
        }

    }
}
