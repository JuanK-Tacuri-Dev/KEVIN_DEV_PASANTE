using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [Repository]
    public class OperatorRepository(IMongoCollection<OperatorEntity> collection) : IOperatorRepository<OperatorEntity>
    {
        private readonly IMongoCollection<OperatorEntity> _collection = collection;

        public Task InsertAsync(OperatorEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(OperatorEntity entity)
        {
            var filter = Builders<OperatorEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<OperatorEntity>.Update
                .Set(m => m.name, entity.name)
                .Set(m => m.operator_type, entity.operator_type)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(OperatorEntity entity)
        {
            var filter = Builders<OperatorEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<OperatorEntity> GetByIdAsync(Expression<Func<OperatorEntity, bool>> specification)
        {
            var filter = Builders<OperatorEntity>.Filter.Where(specification);
            var operatorEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return operatorEntity;
        }

        public async Task<OperatorEntity> GetByCodeAsync(Expression<Func<OperatorEntity, bool>> specification)
        {
            var filter = Builders<OperatorEntity>.Filter.Where(specification);
            var operatorEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return operatorEntity;
        }

        public async Task<IEnumerable<OperatorEntity>> GetByTypeAsync(Expression<Func<OperatorEntity, bool>> specification)
        {
            var filter = Builders<OperatorEntity>.Filter.Where(specification);
            var operatorEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return operatorEntity;
        }

        public async Task<IEnumerable<OperatorEntity>> GetAllAsync(ISpecification<OperatorEntity> specification)
        {
            var filter = Builders<OperatorEntity>.Filter.Where(specification.Criteria);

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<OperatorEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<OperatorEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<OperatorEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

    }
}
