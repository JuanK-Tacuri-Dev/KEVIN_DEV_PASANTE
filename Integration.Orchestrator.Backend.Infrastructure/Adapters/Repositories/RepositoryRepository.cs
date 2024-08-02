﻿using Integration.Orchestrator.Backend.Domain.Entities.Administration;
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
                .Set(m => m.code, entity.code)
                .Set(m => m.port, entity.port)
                .Set(m => m.user, entity.user)
                .Set(m => m.password, entity.password)
                .Set(m => m.data_base_name, entity.data_base_name)
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
            var propertyEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<RepositoryEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<RepositoryEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return propertyEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<RepositoryEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .Limit(specification.Limit)
                .Skip(specification.Skip).CountDocumentsAsync();
        }

    }
}
