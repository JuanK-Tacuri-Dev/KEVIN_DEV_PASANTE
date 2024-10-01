using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [Repository]
    public class RepositoryRepository(IMongoCollection<RepositoryEntity> collection) : IRepositoryRepository<RepositoryEntity>
    {
        private readonly IMongoCollection<RepositoryEntity> _collection = collection;
        
        public Task InsertAsync(RepositoryEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(RepositoryEntity entity)
        {
            var filter = Builders<RepositoryEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<RepositoryEntity>.Update
                .Set(m => m.repository_port, entity.repository_port)
                .Set(m => m.repository_userName, entity.repository_userName)
                .Set(m => m.repository_password, entity.repository_password)
                .Set(m => m.repository_databaseName, entity.repository_databaseName)
                .Set(m => m.auth_type_id, entity.auth_type_id)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(RepositoryEntity entity)
        {
            var filter = Builders<RepositoryEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<RepositoryEntity> GetByIdAsync(Expression<Func<RepositoryEntity, bool>> specification)
        {
            var filter = Builders<RepositoryEntity>.Filter.Where(specification);
            return await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<RepositoryEntity> GetByCodeAsync(Expression<Func<RepositoryEntity, bool>> specification)
        {
            var filter = Builders<RepositoryEntity>.Filter.Where(specification);
            return await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RepositoryEntity>> GetAllAsync(ISpecification<RepositoryEntity> specification)
        {
            var filter = Builders<RepositoryEntity>.Filter.Where(specification.Criteria);

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<RepositoryEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<RepositoryEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }

        public async Task<long> GetTotalRows(ISpecification<RepositoryEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

        public async Task<bool> ValidateDbPortUser(RepositoryEntity entity)
        {
            var filter = Builders<RepositoryEntity>.Filter.And(
                Builders<RepositoryEntity>.Filter.Eq(e => e.repository_databaseName, entity.repository_databaseName),
                Builders<RepositoryEntity>.Filter.Eq(e => e.repository_port, entity.repository_port),
                Builders<RepositoryEntity>.Filter.Eq(e => e.repository_user, entity.repository_user)
            );

            var count = await _collection.Find(filter).CountDocumentsAsync();
            return count >= 1;
        }

    }
}
