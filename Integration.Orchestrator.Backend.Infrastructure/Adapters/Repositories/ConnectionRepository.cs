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
    public class ConnectionRepository(IMongoCollection<ConnectionEntity> collection) 
        : IConnectionRepository<ConnectionEntity>
    {
        private readonly IMongoCollection<ConnectionEntity> _collection = collection;

        public Task InsertAsync(ConnectionEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(ConnectionEntity entity)
        {
            var filter = Builders<ConnectionEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<ConnectionEntity>.Update
                .Set(m => m.server_id, entity.server_id)
                .Set(m => m.adapter_id, entity.adapter_id)
                .Set(m => m.repository_id, entity.repository_id)
                .Set(m => m.connection_name, entity.connection_name)
                .Set(m => m.connection_description, entity.connection_description)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(ConnectionEntity entity)
        {
            var filter = Builders<ConnectionEntity>.Filter.Eq("_id", entity.id);
            return _collection.DeleteOneAsync(filter);
        }

        public async Task<ConnectionEntity> GetByExpressionIdAsync(Expression<Func<ConnectionEntity, bool>> specification)
        {
            var filter = Builders<ConnectionEntity>.Filter.Where(specification);
            var connectionEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return connectionEntity;
        }
        public async Task<ConnectionEntity> GetByIdAsync(Expression<Func<ConnectionEntity, bool>> specification)
        {
            var filter = Builders<ConnectionEntity>.Filter.Where(specification);
            var connectionEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return connectionEntity;
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

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<ConnectionEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<ConnectionEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<ConnectionEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

    }
}
