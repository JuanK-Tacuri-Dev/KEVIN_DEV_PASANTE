using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models.Configurador;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Specifications;
using Integration.Orchestrator.Backend.Infrastructure.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [ExcludeFromCodeCoverage]
    [Repository]
    public class AdapterRepository(IMongoCollection<AdapterEntity> collection) 
        : IAdapterRepository<AdapterEntity>
    {
        private readonly IMongoCollection<AdapterEntity> _collection = collection;
        private Dictionary<string, string> SortMapping = new()
            {
                { "type_id", "CatalogData.catalog_name" }
            };
        public Task InsertAsync(AdapterEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(AdapterEntity entity)
        {
            var filter = Builders<AdapterEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<AdapterEntity>.Update
                .Set(m => m.adapter_name, entity.adapter_name)
                .Set(m => m.type_id, entity.type_id)
                .Set(m => m.adapter_version, entity.adapter_version)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(AdapterEntity entity)
        {
            var filter = Builders<AdapterEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<AdapterEntity> GetByIdAsync(Expression<Func<AdapterEntity, bool>> specification)
        {
            var filter = Builders<AdapterEntity>.Filter.Where(specification);
            var operatorEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return operatorEntity;
        }

        public async Task<AdapterEntity> GetByCodeAsync(Expression<Func<AdapterEntity, bool>> specification)
        {
            var filter = Builders<AdapterEntity>.Filter.Where(specification);
            var operatorEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return operatorEntity;
        }

        public async Task<IEnumerable<AdapterEntity>> GetByTypeAsync(Expression<Func<AdapterEntity, bool>> specification)
        {
            var filter = Builders<AdapterEntity>.Filter.Where(specification);
            var operatorEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return operatorEntity;
        }

        public async Task<IEnumerable<AdapterResponseModel>> GetAllAsync(ISpecification<AdapterEntity> specification)
        {
            var filterBuilder = Builders<AdapterEntity>.Filter;

            // Construir filtro principal desde la especificación
            var filter = filterBuilder.Where(specification.Criteria);

            // Configurar collation
            var collation = new Collation("en", strength: CollationStrength.Secondary);

            // Inicializar el pipeline de agregación con filtro y unwind
            var aggregation = _collection.Aggregate(new AggregateOptions { Collation = collation })
                                         .Match(filter)
                                         .Unwind("CatalogData", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true });

            // Obtener el campo de ordenamiento según la especificación
            string? orderByField = specification.OrderBy != null
                ? SortExpressionConfiguration<AdapterEntity>.GetPropertyName(specification.OrderBy)
                : specification.OrderByDescending != null
                    ? SortExpressionConfiguration<AdapterEntity>.GetPropertyName(specification.OrderByDescending)
                    : null;

            // Configurar el ordenamiento
            var sortDefinition = BsonDocumentExtensions.GetSortDefinition(orderByField, specification.OrderBy != null, this.SortMapping);

            // Aplicar joins si hay especificaciones de include
            if (specification.Includes != null)
            {
                foreach (var join in specification.Includes)
                {
                    aggregation = aggregation.Lookup(join.Collection, join.LocalField, join.ForeignField, join.As);
                }
            }

            if (specification.Skip >= 0)
            {
                aggregation = aggregation.Skip(specification.Skip);
            }

            if (specification.Limit > 0)
            {
                aggregation = aggregation.Limit(specification.Limit);
            }


            aggregation = aggregation.Sort(sortDefinition);

            var result = await aggregation.ToListAsync();


            var data = result.Select(MapToResponseModel);

            return data;
        }

        public async Task<long> GetTotalRows(ISpecification<AdapterEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

        public async Task<bool> ValidateAdapterNameVersion(AdapterEntity entity)
        {
            var filter = Builders<AdapterEntity>.Filter.And(
                Builders<AdapterEntity>.Filter.Eq(e => e.adapter_name, entity.adapter_name),
                Builders<AdapterEntity>.Filter.Eq(e => e.adapter_version, entity.adapter_version),
                Builders<AdapterEntity>.Filter.Ne(e => e.id, entity.id)
            );

            var count = await _collection.Find(filter).CountDocumentsAsync();
            return count >= 1;
        }


        #region Metodos Privados
        private AdapterResponseModel MapToResponseModel(BsonDocument bson)
        {
            return new AdapterResponseModel
            {
                id = bson.GetValueOrDefault("_id", Guid.Empty),
                type_id = bson.GetValueOrDefault("type_id", Guid.Empty),
                status_id = bson.GetValueOrDefault("status_id", Guid.Empty),
                adapter_code = bson.GetValueOrDefault("adapter_code", string.Empty),
                adapter_name = bson.GetValueOrDefault("adapter_name", string.Empty),
                adapter_version = bson.GetValueOrDefault("adapter_version", string.Empty),
                typeAdapterName = bson.GetNestedValueOrDefault("CatalogData", "catalog_name")
            };
        }

        #endregion


    }
}
