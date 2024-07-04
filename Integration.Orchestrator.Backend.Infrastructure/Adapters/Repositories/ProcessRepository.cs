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

        public Task UpdateAsync(ProcessEntity entity)
        {
            var filter = Builders<ProcessEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<ProcessEntity>.Update
                .Set(m => m.process_code, entity.process_code)
                .Set(m => m.process_type, entity.process_type)
                .Set(m => m.connection_id, entity.connection_id)
                .Set(m => m.entities, entity.entities)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(ProcessEntity entity)
        {
            var filter = Builders<ProcessEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<ProcessEntity> GetByIdAsync(Expression<Func<ProcessEntity, bool>> specification)
        {
            var filter = Builders<ProcessEntity>.Filter.Where(specification);
            var processEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return processEntity;
        }

        public async Task<ProcessEntity> GetByCodeAsync(Expression<Func<ProcessEntity, bool>> specification)
        {
            var filter = Builders<ProcessEntity>.Filter.Where(specification);
            var processEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return processEntity;
        }

        public async Task<IEnumerable<ProcessEntity>> GetByTypeAsync(Expression<Func<ProcessEntity, bool>> specification)
        {
            var filter = Builders<ProcessEntity>.Filter.Where(specification);
            var processEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return processEntity;
        }

        public async Task<IEnumerable<ProcessEntity>> GetAllAsync(ISpecification<ProcessEntity> specification)
        {
            var filter = Builders<ProcessEntity>.Filter.Where(specification.Criteria);
            var processEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<ProcessEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<ProcessEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return processEntity;
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
