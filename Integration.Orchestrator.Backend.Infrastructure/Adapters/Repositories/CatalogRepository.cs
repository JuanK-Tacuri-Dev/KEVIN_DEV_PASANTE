using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models.Configurador;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Catalog;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [ExcludeFromCodeCoverage]
    [Repository]
    public class CatalogRepository(IMongoCollection<CatalogEntity> collection)
        : ICatalogRepository<CatalogEntity>
    {
        private readonly IMongoCollection<CatalogEntity> _collection = collection;

        private Dictionary<string, string> SortMappingAndFilter = new()
            {
                { "status_id", "Status.text" }
            };

        public Task InsertAsync(CatalogEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(CatalogEntity entity)
        {
            var filter = Builders<CatalogEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<CatalogEntity>.Update
                .Set(m => m.catalog_code, entity.catalog_code)
                .Set(m => m.catalog_name, entity.catalog_name)
                .Set(m => m.catalog_value, entity.catalog_value)
                .Set(m => m.catalog_detail, entity.catalog_detail)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(CatalogEntity entity)
        {
            var filter = Builders<CatalogEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<CatalogEntity> GetByIdAsync(Expression<Func<CatalogEntity, bool>> specification)
        {
            return await FindByFilter(specification).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CatalogEntity>> GetByFatherAsync(Expression<Func<CatalogEntity, bool>> specification)
        {
            return await FindByFilter(specification).ToListAsync();
        }

        public async Task<CatalogEntity> GetByCodeAsync(Expression<Func<CatalogEntity, bool>> specification)
        {
            return await FindByFilter(specification).FirstOrDefaultAsync();
        }
        public async Task<IEnumerable<CatalogResponseModel>> GetAllAsync(ISpecification<CatalogEntity> specification)
        {
            var filterBuilder = Builders<BsonDocument>.Filter;
            var entityFilterBuilder = Builders<CatalogEntity>.Filter;

            var entityFilter = specification.Criteria != null
                ? entityFilterBuilder.Where(specification.Criteria)
                : entityFilterBuilder.Empty;

            var collation = new Collation("en", strength: CollationStrength.Secondary);
            var aggregation = _collection.Aggregate(new AggregateOptions { Collation = collation })
                                         .Match(entityFilter)
                                         .Unwind("Status", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true });

            specification.Includes?.ForEach(join => aggregation = aggregation.Lookup(join.Collection, join.LocalField, join.ForeignField, join.As));


            if (specification.AdditionalFilters != null && specification.AdditionalFilters.Any())
            {
                foreach (var filterItem in specification.AdditionalFilters)
                {
                    string mappedField = SortMappingAndFilter.TryGetValue(filterItem.Key, out string? value) ? value : filterItem.Key;

                    if (filterItem.Value is IEnumerable<object> values && values.Any())
                    {
                        var valueList = values.Cast<string>().ToList();
                        aggregation = aggregation.Match(filterBuilder.In(mappedField, valueList));
                    }
                    else if (filterItem.Value is string singleValue)
                    {
                        aggregation = aggregation.Match(filterBuilder.Eq(mappedField, singleValue));
                    }
                }
            }
            string? orderByField = specification.OrderBy != null
                ? SortExpressionConfiguration<SynchronizationEntity>.GetPropertyName(specification.OrderBy)
                : specification.OrderByDescending != null
                    ? SortExpressionConfiguration<SynchronizationEntity>.GetPropertyName(specification.OrderByDescending)
                    : null;

            if (!string.IsNullOrEmpty(orderByField))
            {
                var sortDefinition = Builders<BsonDocument>.Sort.Ascending(orderByField);
                aggregation = aggregation.Sort(sortDefinition);
            }

            if (specification.Skip >= 0)
            {
                aggregation = aggregation.
                    Skip(specification.Skip).
                    Limit(specification.Limit);
            }

            var result = await aggregation.ToListAsync();

            return result.Select(doc => BsonSerializer.Deserialize<CatalogResponseModel>(doc)).ToList();

        }




        public async Task<long> GetTotalRows(ISpecification<CatalogEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

        private IFindFluent<CatalogEntity, CatalogEntity> FindByFilter(Expression<Func<CatalogEntity, bool>> specification)
        {
            return _collection.Find(BuildFilter(specification));
        }

        private static FilterDefinition<CatalogEntity> BuildFilter(Expression<Func<CatalogEntity, bool>> specification)
        {
            return Builders<CatalogEntity>.Filter.Where(specification);
        }
    }
}