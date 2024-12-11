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
    public class ProcessRepository(IMongoCollection<ProcessEntity> collection)
        : IProcessRepository<ProcessEntity>
    {
        private readonly IMongoCollection<ProcessEntity> _collection = collection;
        private Dictionary<string, string> SortMapping = new()
            {
                { "process_type_id", "CatalogData.catalog_name" },
                { "connection_id", "ConnectionData.connection_name" }
            };
        public Task InsertAsync(ProcessEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(ProcessEntity entity)
        {
            var filter = Builders<ProcessEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<ProcessEntity>.Update
                .Set(m => m.process_name, entity.process_name)
                .Set(m => m.process_description, entity.process_description)
                .Set(m => m.process_type_id, entity.process_type_id)
                .Set(m => m.connection_id, entity.connection_id)
                .Set(m => m.status_id, entity.status_id)
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

        public async Task<IEnumerable<ProcessResponseModel>> GetAllAsync(ISpecification<ProcessEntity> specification)
        {
            var filterBuilder = Builders<ProcessEntity>.Filter;

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
                ? SortExpressionConfiguration<ServerEntity>.GetPropertyName(specification.OrderBy)
                : specification.OrderByDescending != null
                    ? SortExpressionConfiguration<ServerEntity>.GetPropertyName(specification.OrderByDescending)
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

            aggregation = aggregation.Sort(sortDefinition);

            if (specification.Skip >= 0)
            {
                aggregation = aggregation.Skip(specification.Skip);
            }

            if (specification.Limit > 0)
            {
                aggregation = aggregation.Limit(specification.Limit);
            }

            var result = await aggregation.ToListAsync();


            // Mapear resultados a ServerResponseModel
            var data = result.Select(MapToResponseModel);

            return data;
        }

        public async Task<long> GetTotalRows(ISpecification<ProcessEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

        public async Task<IEnumerable<ProcessEntity>> GetByEntitiesIdAsync(Expression<Func<ProcessEntity, bool>> specification)
        {
            var filter = Builders<ProcessEntity>.Filter.Where(specification);
            var processEntity = await _collection
                .Find(filter).ToListAsync();
            return processEntity;
        }

        public async Task<IEnumerable<ProcessEntity>> GetByPropertiesIdAsync(Expression<Func<ProcessEntity, bool>> specification)
        {
            var filter = Builders<ProcessEntity>.Filter.Where(specification);
            var processEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return processEntity;
        }

        #region Metodos Privados
        private ProcessResponseModel MapToResponseModel(BsonDocument bson)
        {
            return new ProcessResponseModel
            {
                id = bson.GetValueOrDefault("_id", Guid.Empty),
                process_type_id = bson.GetValueOrDefault("process_type_id", Guid.Empty),
                connection_id = bson.GetValueOrDefault("connection_id", Guid.Empty),
                status_id = bson.GetValueOrDefault("status_id", Guid.Empty),
                process_code = bson.GetValueOrDefault("process_code", string.Empty),
                process_name = bson.GetValueOrDefault("process_name", string.Empty),
                process_description = bson.GetValueOrDefault("process_description", string.Empty),
                connectionName = bson.GetNestedValueOrDefault("ConnectionData", "connection_name"),
                typeProcessName = bson.GetNestedValueOrDefault("CatalogData", "catalog_name"),
                entities = bson.TryGetArray("entities", MapToObjectEntity)
            };
        }

        private ObjectEntity MapToObjectEntity(BsonValue entity)
        {
            var bsonEntity = entity.AsBsonDocument;
            return new ObjectEntity
            {
                id = bsonEntity.GetValueOrDefault("_id", Guid.Empty),
                Properties = bsonEntity.TryGetArray("Properties", MapToPropertiesEntity),
                filters = bsonEntity.TryGetArray("filters", MapToFiltersEntity)
            };
        }

        private PropertiesEntity MapToPropertiesEntity(BsonValue property)
        {
            var bsonProperty = property.AsBsonDocument;
            return new PropertiesEntity
            {
                property_id = bsonProperty.GetValueOrDefault("property_id", Guid.Empty),
                internal_status_id = bsonProperty.GetValueOrDefault("internal_status_id", Guid.Empty)
            };
        }

        private FiltersEntity MapToFiltersEntity(BsonValue filter)
        {
            var bsonFilter = filter.AsBsonDocument;
            return new FiltersEntity
            {
                property_id = bsonFilter.GetValueOrDefault("property_id", Guid.Empty),
                operator_id = bsonFilter.GetValueOrDefault("operator_id", Guid.Empty),
                value = bsonFilter.GetValueOrDefault("value", string.Empty)
            };
        }



        #endregion



    }
}
