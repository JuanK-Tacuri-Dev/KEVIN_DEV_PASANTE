using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    public class IntegrationRepository(IMongoCollection<IntegrationEntity> collection) : IIntegrationRepository<IntegrationEntity>
    {
        private readonly IMongoCollection<IntegrationEntity> _collection = collection;
        public Task InsertAsync(IntegrationEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(IntegrationEntity entity)
        {
            var filter = Builders<IntegrationEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<IntegrationEntity>.Update
                .Set(m => m.name, entity.name)
                .Set(m => m.status, entity.status)
                .Set(m => m.observations, entity.observations)
                .Set(m => m.user_id, entity.user_id)
                .Set(m => m.process, entity.process)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(IntegrationEntity entity)
        {
            var filter = Builders<IntegrationEntity>.Filter.Eq("_id", entity.id);
            return _collection.DeleteOneAsync(filter);
        }

        public async Task<IntegrationEntity> GetByIdAsync(Expression<Func<IntegrationEntity, bool>> specification)
        {
            var filter = Builders<IntegrationEntity>.Filter.Where(specification);
            var integrationEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return integrationEntity;
        }

        public async Task<IEnumerable<IntegrationEntity>> GetAllAsync(ISpecification<IntegrationEntity> specification)
        {
            var filter = Builders<IntegrationEntity>.Filter.Where(specification.Criteria);
            var integrationEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<IntegrationEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<IntegrationEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return integrationEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<IntegrationEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .Limit(specification.Limit)
                .Skip(specification.Skip).CountDocumentsAsync();
        }

    }
}
