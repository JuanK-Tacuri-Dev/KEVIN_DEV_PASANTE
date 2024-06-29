using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    public class ValueRepository(IMongoCollection<ValueEntity> collection) : IValueRepository<ValueEntity>
    {
        private readonly IMongoCollection<ValueEntity> _collection = collection;

        public Task InsertAsync(ValueEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(ValueEntity entity)
        {
            var filter = Builders<ValueEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<ValueEntity>.Update
                .Set(m => m.name, entity.name)
                .Set(m => m.value_code, entity.value_code)
                .Set(m => m.value_type, entity.value_type)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }
        
        public async Task DeleteAsync(ValueEntity entity)
        {
            var filter = Builders<ValueEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<ValueEntity> GetByIdAsync(Expression<Func<ValueEntity, bool>> specification)
        {
            var filter = Builders<ValueEntity>.Filter.Where(specification);
            var integrationEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return integrationEntity;
        }

        public async Task<ValueEntity> GetByCodeAsync(Expression<Func<ValueEntity, bool>> specification)
        {
            var filter = Builders<ValueEntity>.Filter.Where(specification);
            var valueEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return valueEntity;
        }

        public async Task<IEnumerable<ValueEntity>> GetByTypeAsync(Expression<Func<ValueEntity, bool>> specification)
        {
            var filter = Builders<ValueEntity>.Filter.Where(specification);
            var valueEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return valueEntity;
        }

        public async Task<IEnumerable<ValueEntity>> GetAllAsync(ISpecification<ValueEntity> specification)
        {
            var filter = Builders<ValueEntity>.Filter.Where(specification.Criteria);
            var valueEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<ValueEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<ValueEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return valueEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<ValueEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .Limit(specification.Limit)
                .Skip(specification.Skip).CountDocumentsAsync();
        }

    }
}
