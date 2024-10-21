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
    public class IntegrationRepository(IMongoCollection<IntegrationEntity> collection) 
        : IIntegrationRepository<IntegrationEntity>
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
                .Set(m => m.integration_name, entity.integration_name)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.integration_observations, entity.integration_observations)
                .Set(m => m.user_id, entity.user_id)
                .Set(m => m.process, entity.process)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(IntegrationEntity entity)
        {
            var filter = Builders<IntegrationEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
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

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<IntegrationEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<IntegrationEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<IntegrationEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

    }
}
