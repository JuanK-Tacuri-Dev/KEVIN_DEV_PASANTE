using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Catalog;
using Integration.Orchestrator.Backend.Domain.Ports.Configurator;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Integration.Orchestrator.Backend.Infrastructure.Services;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

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
                { "isFather", "is_father" },
                { "name", "catalog_name" },
                { "statusId", "status_id" }
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
                .Set(m => m.updated_at, entity.updated_at)
                .Set(m => m.father_code, entity.father_code)
                .Set(m => m.is_father, entity.is_father);
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
            var aggregation = this.GetAllData(specification);

            if (specification.Skip >= 0)
            {
                aggregation = aggregation.
                    Skip(specification.Skip).
                    Limit(specification.Limit);
            }

            var result = await aggregation.ToListAsync();

            return result.Select(doc => BsonSerializer.Deserialize<CatalogResponseModel>(doc)).AsEnumerable();

        }

        private IAggregateFluent<BsonDocument> GetAllData(ISpecification<CatalogEntity> specification)
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

                    var processedValue = BsonDocumentExtensions.ProcessFilterValue(mappedField, filterItem.Value);


                    if (processedValue is List<string> stringList)
                    {
                        aggregation = aggregation.Match(filterBuilder.In(mappedField, stringList));
                    }
                    else if (processedValue is List<bool> boolList)
                    {
                        aggregation = aggregation.Match(filterBuilder.In(mappedField, boolList));
                    }
                    else if (processedValue is List<int> intList)
                    {
                        aggregation = aggregation.Match(filterBuilder.In(mappedField, intList));
                    }
                    else if (processedValue is string strValue)
                    {
                        aggregation = aggregation.Match(filterBuilder.Eq(mappedField, strValue));
                    }
                    else if (processedValue is bool boolValue)
                    {
                        aggregation = aggregation.Match(filterBuilder.Eq(mappedField, boolValue));
                    }
                    else if (processedValue is int intValue)
                    {
                        aggregation = aggregation.Match(filterBuilder.Eq(mappedField, intValue));
                    }
                }

            }
            string? orderByField = specification.OrderBy != null
                ? SortExpressionConfiguration<SynchronizationEntity>.GetPropertyName(specification.OrderBy)
                : specification.OrderByDescending != null
                    ? SortExpressionConfiguration<SynchronizationEntity>.GetPropertyName(specification.OrderByDescending)
                    : null;
            var sortDefinition = BsonDocumentExtensions.GetSortDefinition(orderByField, specification.OrderBy != null, this.SortMappingAndFilter);

            aggregation = aggregation.Sort(sortDefinition);

            return aggregation;

        }

        public async Task<long> GetTotalRows(ISpecification<CatalogEntity> specification)
        {
            var TotalRow = await (this.GetAllData(specification)).ToListAsync();
            return TotalRow.Select(doc => BsonSerializer.Deserialize<CatalogResponseModel>(doc)).Count();
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