using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    public class PropertyRepository(IMongoCollection<PropertyEntity> collection) : IPropertyRepository<PropertyEntity>
    {
        private readonly IMongoCollection<PropertyEntity> _collection = collection;
        public Task InsertAsync(PropertyEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public async Task<PropertyEntity> GetByCodeAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            var filter = Builders<PropertyEntity>.Filter.Where(specification);
            var propertyEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return propertyEntity;
        }

        public async Task<IEnumerable<PropertyEntity>> GetByTypeAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            var filter = Builders<PropertyEntity>.Filter.Where(specification);
            var propertyEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return propertyEntity;
        }

        public async Task<IEnumerable<PropertyEntity>> GetAllAsync(ISpecification<PropertyEntity> specification)
        {
            var filter = Builders<PropertyEntity>.Filter.Where(specification.Criteria);
            var propertyEntity = await _collection
                .Find(filter)
                .Limit(specification.Limit)
                .Skip(specification.Skip)
                .Sort(specification.OrderBy != null
                                               ? Builders<PropertyEntity>.Sort.Ascending(specification.OrderBy)
                                               : Builders<PropertyEntity>.Sort.Descending(specification.OrderByDescending))
                .ToListAsync();
            return propertyEntity;
        }

        public async Task<long> GetTotalRows(ISpecification<PropertyEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .Limit(specification.Limit)
                .Skip(specification.Skip).CountDocumentsAsync();
        }

    }
}
