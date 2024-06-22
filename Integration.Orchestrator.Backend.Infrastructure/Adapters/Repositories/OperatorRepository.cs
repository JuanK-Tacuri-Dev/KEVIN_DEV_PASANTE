using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    public class OperatorRepository(IMongoCollection<OperatorEntity> collection) : IOperatorRepository<OperatorEntity>
    {
        private readonly IMongoCollection<OperatorEntity> _collection = collection;
        public Task InsertAsync(OperatorEntity entity)
        {
            return _collection.InsertOneAsync(entity);
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
            var operatorEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<OperatorEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<OperatorEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return operatorEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<OperatorEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .Limit(specification.Limit)
                .Skip(specification.Skip).CountDocumentsAsync();
        }

    }
}
