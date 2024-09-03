using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [Repository]
    public class CatalogRepository(IMongoCollection<CatalogEntity> collection) : ICatalogRepository<CatalogEntity>
    {
        private readonly IMongoCollection<CatalogEntity> _collection = collection;
        
        public Task InsertAsync(CatalogEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(CatalogEntity entity)
        {
            var filter = Builders<CatalogEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<CatalogEntity>.Update
                .Set(m => m.catalog_name, entity.catalog_name)
                .Set(m => m.catalog_value, entity.catalog_value)
                .Set(m => m.father_code, entity.father_code)
                .Set(m => m.catalog_detail, entity.catalog_detail)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(CatalogEntity entity)
        {
            var filter = Builders<CatalogEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<CatalogEntity> GetByIdAsync(Expression<Func<CatalogEntity, bool>> specification)
        {
            var filter = Builders<CatalogEntity>.Filter.Where(specification);
            var catalogEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return catalogEntity;
        }

        public async Task<IEnumerable<CatalogEntity>> GetByFatherAsync(Expression<Func<CatalogEntity, bool>> specification)
        {
            var filter = Builders<CatalogEntity>.Filter.Where(specification);
            var catalogEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return catalogEntity;
        }

        public async Task<CatalogEntity> GetByCodeAsync(Expression<Func<CatalogEntity, bool>> specification)
        {
            var filter = Builders<CatalogEntity>.Filter.Where(specification);
            var catalogEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return catalogEntity;
        }

        public async Task<IEnumerable<CatalogEntity>> GetAllAsync(ISpecification<CatalogEntity> specification)
        {
            var filter = Builders<CatalogEntity>.Filter.Where(specification.Criteria);

            var query =  _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<CatalogEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<CatalogEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<CatalogEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

    }
}
