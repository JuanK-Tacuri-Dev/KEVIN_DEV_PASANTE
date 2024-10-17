using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
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
                .Set(m => m.property_name, entity.property_name)
                .Set(m => m.type_id, entity.type_id)
                .Set(m => m.entity_id, entity.entity_id)
                .Set(m => m.status_id, entity.status_id)
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
            return await FindByFilter(specification).FirstOrDefaultAsync();
        }

        public async Task<PropertyEntity> GetByCodeAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            return await FindByFilter(specification).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PropertyEntity>> GetByTypeAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            return await FindByFilter(specification).ToListAsync();
        }

        public async Task<IEnumerable<PropertyEntity>> GetByEntityAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            return await FindByFilter(specification).ToListAsync();
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

        public async Task<IEnumerable<PropertyEntity>> GetByNameAndEntityIdAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            return await FindByFilter(specification).ToListAsync();
        }

        private IFindFluent<PropertyEntity, PropertyEntity> FindByFilter(Expression<Func<PropertyEntity, bool>> specification)
        {
            return _collection.Find(BuildFilter(specification));
        }

        private static FilterDefinition<PropertyEntity> BuildFilter(Expression<Func<PropertyEntity, bool>> specification)
        {
            return Builders<PropertyEntity>.Filter.Where(specification);
        }
        public async Task<bool> GetByExits(PropertyEntity property)
        {
            FilterDefinition<PropertyEntity> filters;
            
                filters = Builders<PropertyEntity>.Filter.And(
                    Builders<PropertyEntity>.Filter.Eq(e => e.property_name, property.property_name),
                    Builders<PropertyEntity>.Filter.Eq(e => e.property_code, property.property_code),
                    Builders<PropertyEntity>.Filter.Eq(e => e.type_id, property.type_id),
                    Builders<PropertyEntity>.Filter.Eq(e => e.status_id, property.status_id),
                    Builders<PropertyEntity>.Filter.Ne(e => e.id, property.id)
                );

            var count = await _collection.Find(filters).CountDocumentsAsync();
            return count >= 1;
        }
    }
}
