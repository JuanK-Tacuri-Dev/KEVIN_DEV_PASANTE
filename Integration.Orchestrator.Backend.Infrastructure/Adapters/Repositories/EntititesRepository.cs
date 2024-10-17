using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [ExcludeFromCodeCoverage]
    [Repository]
    public class EntitiesRepository(IMongoCollection<EntitiesEntity> collection)
        : IEntitiesRepository<EntitiesEntity>
    {
        private readonly IMongoCollection<EntitiesEntity> _collection = collection;

        public Task InsertAsync(EntitiesEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(EntitiesEntity entity)
        {
            var filter = Builders<EntitiesEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<EntitiesEntity>.Update
                .Set(m => m.entity_name, entity.entity_name)
                .Set(m => m.type_id, entity.type_id)
                .Set(m => m.repository_id, entity.repository_id)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(EntitiesEntity entity)
        {
            var filter = Builders<EntitiesEntity>.Filter.Eq("_id", entity.id);
            return _collection.DeleteOneAsync(filter);
        }

        public async Task<EntitiesEntity> GetByIdAsync(Expression<Func<EntitiesEntity, bool>> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification);
            var entitiesEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return entitiesEntity;
        }

        public async Task<EntitiesEntity> GetByCodeAsync(Expression<Func<EntitiesEntity, bool>> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification);
            var entitiesEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return entitiesEntity;
        }

        public async Task<IEnumerable<EntitiesEntity>> GetByTypeIdAsync(Expression<Func<EntitiesEntity, bool>> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification);
            var entitiesEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return entitiesEntity;
        }

        public async Task<IEnumerable<EntitiesEntity>> GetByRepositoryIdAsync(Expression<Func<EntitiesEntity, bool>> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification);
            var entitiesEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return entitiesEntity;
        }
        public async Task<bool> GetByExits(EntitiesEntity entity)
        {
            FilterDefinition<EntitiesEntity> filters;
            filters = Builders<EntitiesEntity>.Filter.And(
                Builders<EntitiesEntity>.Filter.Eq(e => e.entity_name, entity.entity_name),
                Builders<EntitiesEntity>.Filter.Eq(e => e.type_id, entity.type_id),
                Builders<EntitiesEntity>.Filter.Eq(e => e.repository_id, entity.repository_id),
                Builders<EntitiesEntity>.Filter.Eq(e => e.status_id, entity.status_id),
                Builders<EntitiesEntity>.Filter.Ne(e => e.id, entity.id)
            );

            var count = await _collection.Find(filters).CountDocumentsAsync();
            return count >= 1;
        }

        public async Task<IEnumerable<EntitiesEntity>> GetAllAsync(ISpecification<EntitiesEntity> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification.Criteria);

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<EntitiesEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<EntitiesEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<EntitiesEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

        public async Task<IEnumerable<EntitiesEntity>> GetByNameAndRepositoryIdAsync(Expression<Func<EntitiesEntity, bool>> specification)
        {
            return await _collection
                .Find(Builders<EntitiesEntity>.Filter.Where(specification))
                .ToListAsync();
        }
    }
}
