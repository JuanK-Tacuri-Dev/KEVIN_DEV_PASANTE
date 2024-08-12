using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [Repository]
    public class StatusRepository(IMongoCollection<StatusEntity> collection) : IStatusRepository<StatusEntity>
    {
        private readonly IMongoCollection<StatusEntity> _collection = collection;

        public Task InsertAsync(StatusEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(StatusEntity entity)
        {
            var filter = Builders<StatusEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<StatusEntity>.Update
                .Set(m => m.key, entity.key)
                .Set(m => m.text, entity.text)
                .Set(m => m.color, entity.color)
                .Set(m => m.background, entity.background)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(StatusEntity entity)
        {
            var filter = Builders<StatusEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<StatusEntity> GetByIdAsync(Expression<Func<StatusEntity, bool>> specification)
        {
            var filter = Builders<StatusEntity>.Filter.Where(specification);
            var statusEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return statusEntity;
        }

        public async Task<StatusEntity> GetByKeyAsync(Expression<Func<StatusEntity, bool>> specification)
        {
            var filter = Builders<StatusEntity>.Filter.Where(specification);
            var statusEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return statusEntity;
        }

        public async Task<IEnumerable<StatusEntity>> GetAllAsync(ISpecification<StatusEntity> specification)
        {
            var filter = Builders<StatusEntity>.Filter.Where(specification.Criteria);

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<StatusEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<StatusEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<StatusEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

    }
}
