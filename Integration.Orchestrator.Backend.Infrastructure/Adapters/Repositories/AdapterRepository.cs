using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    public class AdapterRepository(IMongoCollection<AdapterEntity> collection) : IAdapterRepository<AdapterEntity>
    {
        private readonly IMongoCollection<AdapterEntity> _collection = collection;
        
        public Task InsertAsync(AdapterEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(AdapterEntity entity)
        {
            var filter = Builders<AdapterEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<AdapterEntity>.Update
                .Set(m => m.name, entity.name)
                .Set(m => m.adapter_code, entity.adapter_code)
                .Set(m => m.adapter_type, entity.adapter_type)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(AdapterEntity entity)
        {
            var filter = Builders<AdapterEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<AdapterEntity> GetByIdAsync(Expression<Func<AdapterEntity, bool>> specification)
        {
            var filter = Builders<AdapterEntity>.Filter.Where(specification);
            var operatorEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return operatorEntity;
        }

        public async Task<AdapterEntity> GetByCodeAsync(Expression<Func<AdapterEntity, bool>> specification)
        {
            var filter = Builders<AdapterEntity>.Filter.Where(specification);
            var operatorEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return operatorEntity;
        }

        public async Task<IEnumerable<AdapterEntity>> GetByTypeAsync(Expression<Func<AdapterEntity, bool>> specification)
        {
            var filter = Builders<AdapterEntity>.Filter.Where(specification);
            var operatorEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return operatorEntity;
        }

        public async Task<IEnumerable<AdapterEntity>> GetAllAsync(ISpecification<AdapterEntity> specification)
        {
            var filter = Builders<AdapterEntity>.Filter.Where(specification.Criteria);
            var operatorEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<AdapterEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<AdapterEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return operatorEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<AdapterEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .Limit(specification.Limit)
                .Skip(specification.Skip).CountDocumentsAsync();
        }

    }
}
