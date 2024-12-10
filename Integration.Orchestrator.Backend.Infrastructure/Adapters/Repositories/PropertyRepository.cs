using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Entity;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Property;
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
    public class PropertyRepository(IMongoCollection<PropertyEntity> collection) 
        : IPropertyRepository<PropertyEntity>
    {
        private readonly IMongoCollection<PropertyEntity> _collection = collection;

        private Dictionary<string, string> SortMapping = new()
        {
            { "type_id", "CatalogData.catalog_name" },
            { "entity_id", "EntityData.entity_name" }
        };
        public Task InsertAsync(PropertyEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(PropertyEntity entity)
        {
            var filter = Builders<PropertyEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<PropertyEntity>.Update
                .Set(m => m.property_name, entity.property_name)
                .Set(m => m.type_id, entity.type_id)
                .Set(m => m.entity_id, entity.entity_id)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(PropertyEntity entity)
        {
            var filter = Builders<PropertyEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<PropertyEntity> GetByIdAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            return await FindByFilter(specification).FirstOrDefaultAsync();
        }

        public async Task<PropertyEntity> GetByCodeAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            return await FindByFilter(specification).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<PropertyEntity>> GetByTypeAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            return await FindByFilter(specification).ToListAsync();
        }

        public async Task<IEnumerable<PropertyEntity>> GetByEntitysAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            return await FindByFilter(specification).ToListAsync();
        }

        public async Task<PropertyEntity> GetByEntityAsync(Expression<Func<PropertyEntity, bool>> specification)
        {
            var filter = Builders<PropertyEntity>.Filter.Where(specification);
            var processEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return processEntity;
        }

        public async Task<IEnumerable<PropertyResponseModel>> GetAllAsync(ISpecification<PropertyEntity> specification)
        {
            var filter = Builders<PropertyEntity>.Filter.Where(specification.Criteria);

            var collation = new Collation("en", strength: CollationStrength.Secondary);

            // Inicializar el pipeline de agregación con filtro y unwind
            var aggregation = _collection.Aggregate(new AggregateOptions { Collation = collation })
                                         .Match(filter)
                                         .Unwind("CatalogData", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true })
                                         .Unwind("EntityData", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true });

            // Obtener el campo de ordenamiento según la especificación
            string? orderByField = specification.OrderBy != null
                ? SortExpressionConfiguration<PropertyEntity>.GetPropertyName(specification.OrderBy)
                : specification.OrderByDescending != null
                    ? SortExpressionConfiguration<PropertyEntity>.GetPropertyName(specification.OrderByDescending)
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

        private PropertyResponseModel MapToResponseModel(BsonDocument bson)
        {
            return new PropertyResponseModel
            {
                id = bson.GetValueOrDefault("_id", Guid.Empty),
                property_code = bson.GetValueOrDefault("entity_code", string.Empty),
                property_name = bson.GetValueOrDefault("property_name", string.Empty),
                type_id = bson.GetValueOrDefault("type_id", Guid.Empty),
                typePropertyName = bson.GetNestedValueOrDefault("CatalogData", "catalog_name"),
                entity_id = bson.GetValueOrDefault("entity_id", Guid.Empty),
                entityName = bson.GetNestedValueOrDefault("EntityData", "entity_name"),
            };
        }

        public async Task<long> GetTotalRows(ISpecification<PropertyEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

        private IFindFluent<PropertyEntity, PropertyEntity> FindByFilter(Expression<Func<PropertyEntity, bool>> specification)
        {
            return _collection.Find(BuildFilter(specification));
        }

        private static FilterDefinition<PropertyEntity> BuildFilter(Expression<Func<PropertyEntity, bool>> specification)
        {
            return Builders<PropertyEntity>.Filter.Where(specification);
        }
        public async Task<bool> ValidateNameAndEntity(PropertyEntity property)
        {
            FilterDefinition<PropertyEntity> filters;
            
                filters = Builders<PropertyEntity>.Filter.And(
                    Builders<PropertyEntity>.Filter.Eq(e => e.property_name, property.property_name),
                    Builders<PropertyEntity>.Filter.Eq(e => e.entity_id, property.entity_id),
                    Builders<PropertyEntity>.Filter.Ne(e => e.id, property.id)
                );

            var count = await _collection.Find(filters).CountDocumentsAsync();
            return count >= 1;
        }


    }
}
