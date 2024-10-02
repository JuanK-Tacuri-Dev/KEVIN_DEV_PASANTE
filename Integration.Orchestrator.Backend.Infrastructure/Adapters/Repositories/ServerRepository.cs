using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [Repository]
    public class ServerRepository(IMongoCollection<ServerEntity> collection) : IServerRepository<ServerEntity>
    {
        private readonly IMongoCollection<ServerEntity> _collection = collection;

        public Task InsertAsync(ServerEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(ServerEntity entity)
        {
            var filter = Builders<ServerEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<ServerEntity>.Update
                .Set(m => m.server_name, entity.server_name)
                .Set(m => m.type_id, entity.type_id)
                .Set(m => m.server_url, entity.server_url)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(ServerEntity entity)
        {
            var filter = Builders<ServerEntity>.Filter.Eq("_id", entity.id);
            return _collection.DeleteOneAsync(filter);
        }

        public async Task<ServerEntity> GetByIdAsync(Expression<Func<ServerEntity, bool>> specification)
        {
            var filter = Builders<ServerEntity>.Filter.Where(specification);
            var serverEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return serverEntity;
        }

        public async Task<ServerEntity> GetByCodeAsync(Expression<Func<ServerEntity, bool>> specification)
        {
            var filter = Builders<ServerEntity>.Filter.Where(specification);
            var serverEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return serverEntity;
        }

        public async Task<IEnumerable<ServerEntity>> GetByTypeAsync(Expression<Func<ServerEntity, bool>> specification)
        {
            var filter = Builders<ServerEntity>.Filter.Where(specification);
            var serverEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return serverEntity;
        }

        public async Task<IEnumerable<ServerEntity>> GetAllAsync(ISpecification<ServerEntity> specification)
        {
            var filter = Builders<ServerEntity>.Filter.Where(specification.Criteria);

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<ServerEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<ServerEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<ServerEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }
        public async Task<bool> ValidateNameURL(ServerEntity entity)
        {
            var filter = Builders<ServerEntity>.Filter.And(
                Builders<ServerEntity>.Filter.Eq(e => e.server_name, entity.server_name),
                Builders<ServerEntity>.Filter.Eq(e => e.server_url, entity.server_url)
            );

            var count = await _collection.Find(filter).CountDocumentsAsync();
            return count >= 1;
        }
    }
}
