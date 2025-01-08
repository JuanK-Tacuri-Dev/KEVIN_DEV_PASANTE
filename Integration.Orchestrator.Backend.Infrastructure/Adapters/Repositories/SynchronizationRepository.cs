﻿using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Catalog;
using Integration.Orchestrator.Backend.Domain.Models.Configurator;
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
    public class SynchronizationRepository(IMongoCollection<SynchronizationEntity> collection)
        : ISynchronizationRepository<SynchronizationEntity>
    {
        private readonly IMongoCollection<SynchronizationEntity> _collection = collection;

        private Dictionary<string, string> SortMapping = new()
            {
                { "statusid", "SynchronizationStates.synchronization_status_key" }
            };

        public Task InsertAsync(SynchronizationEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(SynchronizationEntity entity)
        {
            var filter = Builders<SynchronizationEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<SynchronizationEntity>.Update
                .Set(m => m.synchronization_name, entity.synchronization_name)
                .Set(m => m.franchise_id, entity.franchise_id)
                .Set(m => m.synchronization_observations, entity.synchronization_observations)
                .Set(m => m.user_id, entity.user_id)
                .Set(m => m.synchronization_hour_to_execute, entity.synchronization_hour_to_execute)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.integrations, entity.integrations)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(SynchronizationEntity entity)
        {
            var filter = Builders<SynchronizationEntity>.Filter.Eq("_id", entity.id);
            return _collection.DeleteOneAsync(filter);
        }

        public async Task<SynchronizationEntity> GetByIdAsync(Expression<Func<SynchronizationEntity, bool>> specification)
        {
            var filter = Builders<SynchronizationEntity>.Filter.Where(specification);
            var synchronizationEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return synchronizationEntity;
        }

        public async Task<SynchronizationEntity> GetByCodeAsync(Expression<Func<SynchronizationEntity, bool>> specification)
        {
            var filter = Builders<SynchronizationEntity>.Filter.Where(specification);
            var entityFound = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return entityFound;
        }

        public async Task<IEnumerable<SynchronizationEntity>> GetByFranchiseIdAsync(Expression<Func<SynchronizationEntity, bool>> specification)
        {
            var filter = Builders<SynchronizationEntity>.Filter.Where(specification);
            var synchronizationEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return synchronizationEntity;
        }

        public async Task<IEnumerable<SynchronizationResponseModel>> GetAllAsync(ISpecification<SynchronizationEntity> specification)
        {
            var aggregation = this.GetAllData(specification);

            if (specification.Skip >= 0)
            {
                aggregation = aggregation.
                    Skip(specification.Skip).
                    Limit(specification.Limit);
            }

            var result = await aggregation.ToListAsync();

            return result.Select(doc => BsonSerializer.Deserialize<SynchronizationResponseModel>(doc)).AsEnumerable();
        }


        private IAggregateFluent<BsonDocument> GetAllData(ISpecification<SynchronizationEntity> specification)
        {
            var filterBuilder = Builders<BsonDocument>.Filter;
            var entityFilterBuilder = Builders<SynchronizationEntity>.Filter;

            var entityFilter = specification.Criteria != null
                ? entityFilterBuilder.Where(specification.Criteria)
                : entityFilterBuilder.Empty;

            var collation = new Collation("en", strength: CollationStrength.Secondary);
            var aggregation = _collection.Aggregate(new AggregateOptions { Collation = collation })
                                         .Match(entityFilter)
                                         .Unwind("SynchronizationStates", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true });

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
                ? SortExpressionConfiguration<SynchronizationEntity>.GetPropertyName(specification.OrderBy)
                : specification.OrderByDescending != null
                    ? SortExpressionConfiguration<SynchronizationEntity>.GetPropertyName(specification.OrderByDescending)
                    : null;

            var sortDefinition = BsonDocumentExtensions.GetSortDefinition(orderByField, specification.OrderBy != null, this.SortMapping);

            aggregation = aggregation.Sort(sortDefinition);

            return aggregation;
        }



        public async Task<long> GetTotalRows(ISpecification<SynchronizationEntity> specification)
        {
            var TotalRow = await (this.GetAllData(specification)).ToListAsync();
            return TotalRow.Select(doc => BsonSerializer.Deserialize<SynchronizationResponseModel>(doc)).Count();
        }

    }
}
