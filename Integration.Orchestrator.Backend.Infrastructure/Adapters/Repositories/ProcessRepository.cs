using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    public class ProcessRepository(IMongoCollection<ProcessEntity> collection) : IProcessRepository<ProcessEntity>
    {
        private readonly IMongoCollection<ProcessEntity> _collection = collection;
        public Task InsertAsync(ProcessEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public async Task<ProcessEntity> GetByCodeAsync(Expression<Func<ProcessEntity, bool>> specification)
        {
            var filter = Builders<ProcessEntity>.Filter.Where(specification);
            var synchronizationEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return synchronizationEntity;
        }

        public async Task<IEnumerable<ProcessEntity>> GetByTypeAsync(Expression<Func<ProcessEntity, bool>> specification)
        {
            var filter = Builders<ProcessEntity>.Filter.Where(specification);
            var synchronizationEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return synchronizationEntity;
        }

        public async Task<IEnumerable<ProcessEntity>> GetAllAsync(ISpecification<ProcessEntity> specification)
        {
            var filter = Builders<ProcessEntity>.Filter.Where(specification.Criteria);
            var synchronizationEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<ProcessEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<ProcessEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return synchronizationEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<ProcessEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .Limit(specification.Limit)
                .Skip(specification.Skip).CountDocumentsAsync();
        }

    }
}
