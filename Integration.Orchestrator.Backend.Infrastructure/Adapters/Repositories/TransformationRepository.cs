using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Catalog;
using Integration.Orchestrator.Backend.Domain.Models.Configurator;
using Integration.Orchestrator.Backend.Domain.Models.Configurator.Transformation;
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
    public class TransformationRepository(IMongoCollection<TransformationEntity> collection)
        : ITransformationRepository<TransformationEntity>
    {
        private readonly IMongoCollection<TransformationEntity> _collection = collection;

        private Dictionary<string, string> SortMapping = new()
            {
            { "code", "transformation_code" },
            { "name", "transformation_name" },
            { "description", "description" },
            };

        public async Task<IEnumerable<TransformationResponseModel>> GetAllAsync(ISpecification<TransformationEntity> specification)
        {
            var aggregation = this.GetAllData(specification);

            if (specification.Skip >= 0)
            {
                aggregation = aggregation.
                    Skip(specification.Skip).
                    Limit(specification.Limit);
            }

            var result = await aggregation.ToListAsync();

            return result.Select(doc => BsonSerializer.Deserialize<TransformationResponseModel>(doc)).AsEnumerable();
        }



        private IAggregateFluent<BsonDocument> GetAllData(ISpecification<TransformationEntity> specification)
        {
            var filterBuilder = Builders<BsonDocument>.Filter;


            var entityFilterBuilder = Builders<TransformationEntity>.Filter;

            var entityFilter = specification.Criteria != null
                ? entityFilterBuilder.Where(specification.Criteria)
                : entityFilterBuilder.Empty;

            var collation = new Collation("en", strength: CollationStrength.Secondary);
            var aggregations = _collection.Aggregate(new AggregateOptions { Collation = collation })
                             .Match(entityFilter);

            var aggregation = _collection.Aggregate(new AggregateOptions { Collation = collation })
                                         .Match(entityFilter)
                                         .Unwind("Status", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true });




            specification.Includes?.ForEach(join => aggregation = aggregation.Lookup(join.Collection, join.LocalField, join.ForeignField, join.As));


            if (specification.AdditionalFilters != null && specification.AdditionalFilters.Any())
            {
                foreach (var filterItem in specification.AdditionalFilters)
                {
                    string mappedField = SortMapping.TryGetValue(filterItem.Key, out string? value) ? value : filterItem.Key;

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
                ? SortExpressionConfiguration<TransformationEntity>.GetPropertyName(specification.OrderBy)
                : specification.OrderByDescending != null
                    ? SortExpressionConfiguration<TransformationEntity>.GetPropertyName(specification.OrderByDescending)
                    : null;

            var sortDefinition = BsonDocumentExtensions.GetSortDefinition(orderByField, specification.OrderBy != null, this.SortMapping);

            aggregation = aggregation.Sort(sortDefinition);

            return aggregation;
        }



        public async Task<long> GetTotalRowsAsync(ISpecification<TransformationEntity> specification)
        {
            var TotalRow = await (this.GetAllData(specification)).ToListAsync();
            return TotalRow.Select(doc => BsonSerializer.Deserialize<TransformationResponseModel>(doc)).Count();
        }

    }
}
