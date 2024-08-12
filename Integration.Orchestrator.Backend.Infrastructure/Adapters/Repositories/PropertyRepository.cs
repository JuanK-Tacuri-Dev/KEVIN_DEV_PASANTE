using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [Repository]
    public class PropertyRepository(IMongoCollection<PropertyEntity> collection) : IPropertyRepository<PropertyEntity>
    {
        private readonly IMongoCollection<PropertyEntity> _collection = collection;
        
        public Task InsertAsync(PropertyEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(PropertyEntity entity)
        {
            var filter = Builders<PropertyEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<PropertyEntity>.Update
                .Set(m => m.name, entity.name)
                .Set(m => m.property_code, entity.property_code)
                .Set(m => m.property_type, entity.property_type)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(PropertyEntity entity)
        {
            var filter = Builders<PropertyEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<PropertyEntity> GetByIdAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            var filter = Builders<PropertyEntity>.Filter.Where(specification);
            var propertyEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return propertyEntity;
        }

        public async Task<PropertyEntity> GetByCodeAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            var filter = Builders<PropertyEntity>.Filter.Where(specification);
            var propertyEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return propertyEntity;
        }

        public async Task<IEnumerable<PropertyEntity>> GetByTypeAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            var filter = Builders<PropertyEntity>.Filter.Where(specification);
            var propertyEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return propertyEntity;
        }

        public async Task<IEnumerable<PropertyEntity>> GetAllAsync(ISpecification<PropertyEntity> specification)
        {
            var filter = Builders<PropertyEntity>.Filter.Where(specification.Criteria);

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<PropertyEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<PropertyEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<PropertyEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

    }
}
