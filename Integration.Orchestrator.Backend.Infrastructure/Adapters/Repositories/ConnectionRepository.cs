using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    public class ConnectionRepository(IMongoCollection<ConnectionEntity> collection) : IConnectionRepository<ConnectionEntity>
    {
        private readonly IMongoCollection<ConnectionEntity> _collection = collection;
        public Task InsertAsync(ConnectionEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public async Task<ConnectionEntity> GetByCodeAsync(Expression<Func<ConnectionEntity, bool>> specification)
        {
            var filter = Builders<ConnectionEntity>.Filter.Where(specification);
            var connectionEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return connectionEntity;
        }

        public async Task<IEnumerable<ConnectionEntity>> GetByTypeAsync(Expression<Func<ConnectionEntity, bool>> specification)
        {
            var filter = Builders<ConnectionEntity>.Filter.Where(specification);
            var connectionEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return connectionEntity;
        }

        public async Task<IEnumerable<ConnectionEntity>> GetAllAsync(ISpecification<ConnectionEntity> specification)
        {
            var filter = Builders<ConnectionEntity>.Filter.Where(specification.Criteria);
            var connectionEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<ConnectionEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<ConnectionEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return connectionEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<ConnectionEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .Limit(specification.Limit)
                .Skip(specification.Skip).CountDocumentsAsync();
        }

    }
}
