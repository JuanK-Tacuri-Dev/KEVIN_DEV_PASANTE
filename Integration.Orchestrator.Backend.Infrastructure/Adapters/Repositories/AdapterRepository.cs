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
    public class AdapterRepository(IMongoCollection<AdapterEntity> collection) 
        : IAdapterRepository<AdapterEntity>
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
                .Set(m => m.adapter_name, entity.adapter_name)
                .Set(m => m.type_id, entity.type_id)
                .Set(m => m.adapter_version, entity.adapter_version)
                .Set(m => m.status_id, entity.status_id)
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

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<AdapterEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<AdapterEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<AdapterEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

        public async Task<bool> ValidateAdapterNameVersion(AdapterEntity entity)
        {
            var filter = Builders<AdapterEntity>.Filter.And(
                Builders<AdapterEntity>.Filter.Eq(e => e.adapter_name, entity.adapter_name),
                Builders<AdapterEntity>.Filter.Eq(e => e.adapter_version, entity.adapter_version),
                Builders<AdapterEntity>.Filter.Ne(e => e.id, entity.id)
            );

            var count = await _collection.Find(filter).CountDocumentsAsync();
            return count >= 1;
        }

    }
}
