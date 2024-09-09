using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [Repository]
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
                .Set(m => m.value_name, entity.value_name)
                .Set(m => m.type_id, entity.type_id)
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
            var valueEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return valueEntity;
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

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<ValueEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<ValueEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<ValueEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

    }
}
