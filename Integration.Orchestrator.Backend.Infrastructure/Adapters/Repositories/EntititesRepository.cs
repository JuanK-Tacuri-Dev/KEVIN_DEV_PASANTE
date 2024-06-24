using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    public class EntitiesRepository(IMongoCollection<EntitiesEntity> collection) : IEntitiesRepository<EntitiesEntity>
    {
        private readonly IMongoCollection<EntitiesEntity> _collection = collection;
        public Task InsertAsync(EntitiesEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public async Task<EntitiesEntity> GetByCodeAsync(Expression<Func<EntitiesEntity, bool>> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification);
            var entitiesEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return entitiesEntity;
        }

        public async Task<IEnumerable<EntitiesEntity>> GetByTypeAsync(Expression<Func<EntitiesEntity, bool>> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification);
            var entitiesEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return entitiesEntity;
        }

        public async Task<IEnumerable<EntitiesEntity>> GetAllAsync(ISpecification<EntitiesEntity> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification.Criteria);
            var entitiesEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<EntitiesEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<EntitiesEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return entitiesEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<EntitiesEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .Limit(specification.Limit)
                .Skip(specification.Skip).CountDocumentsAsync();
        }

    }
}
