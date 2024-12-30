using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Models.Configurator;
using Integration.Orchestrator.Backend.Domain.Models.Configurator.Entity;
using Integration.Orchestrator.Backend.Domain.Ports.Configurator;
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
    public class EntitiesRepository(IMongoCollection<EntitiesEntity> collection)
        : IEntitiesRepository<EntitiesEntity>
    {
        private readonly IMongoCollection<EntitiesEntity> _collection = collection;

        private Dictionary<string, string> SortMapping = new()
            {
                { "type_id", "CatalogData.catalog_name" },
                { "repository_id", "RepositoryData.repository_databaseName" }
            };

        public Task InsertAsync(EntitiesEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(EntitiesEntity entity)
        {
            var filter = Builders<EntitiesEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<EntitiesEntity>.Update
                .Set(m => m.entity_name, entity.entity_name)
                .Set(m => m.type_id, entity.type_id)
                .Set(m => m.repository_id, entity.repository_id)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(EntitiesEntity entity)
        {
            var filter = Builders<EntitiesEntity>.Filter.Eq("_id", entity.id);
            return _collection.DeleteOneAsync(filter);
        }

        public async Task<EntitiesEntity> GetByIdAsync(Expression<Func<EntitiesEntity, bool>> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification);
            var entitiesEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return entitiesEntity;
        }

        public async Task<EntitiesEntity> GetByCodeAsync(Expression<Func<EntitiesEntity, bool>> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification);
            var entitiesEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return entitiesEntity;
        }

        public async Task<IEnumerable<EntitiesEntity>> GetByTypeIdAsync(Expression<Func<EntitiesEntity, bool>> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification);
            var entitiesEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return entitiesEntity;
        }

        public async Task<IEnumerable<EntitiesEntity>> GetByRepositoryIdAsync(Expression<Func<EntitiesEntity, bool>> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification);
            var entitiesEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return entitiesEntity;
        }
        
        public async Task<bool> GetRepositoryAndNameExists(EntitiesEntity entity)
        {
            FilterDefinition<EntitiesEntity> filters;
            filters = Builders<EntitiesEntity>.Filter.And(
                Builders<EntitiesEntity>.Filter.Eq(e => e.entity_name, entity.entity_name),
                Builders<EntitiesEntity>.Filter.Eq(e => e.repository_id, entity.repository_id),
                Builders<EntitiesEntity>.Filter.Ne(e => e.id, entity.id)
            );

            var count = await _collection.Find(filters).CountDocumentsAsync();
            return count >= 1;
        }

        public async Task<IEnumerable<EntityResponseModel>> GetAllAsync(ISpecification<EntitiesEntity> specification)
        {
            var filter = Builders<EntitiesEntity>.Filter.Where(specification.Criteria);

            // Configurar collation
            var collation = new Collation("en", strength: CollationStrength.Secondary);

            // Inicializar el pipeline de agregación con filtro y unwind
            var aggregation = _collection.Aggregate(new AggregateOptions { Collation = collation })
                                         .Match(filter)
                                         .Unwind("CatalogData", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true })
                                         .Unwind("RepositoryData", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true });

            /*   .Match(filter)
               .Lookup(
                   foreignCollectionName: "Integration_Catalog",
                   localField: "type_id",
                   foreignField: "_id",
                   @as: "CatalogData"
               ).Lookup(
                   foreignCollectionName: "Integration_Repository",
                   localField: "repository_id",
                   foreignField: "_id",
                   @as: "RepositoryData"
               )*/

            // Definición de ordenamiento
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

            var data = result.Select(MapToResponseModel);

            return data;
        }

        private EntityResponseModel MapToResponseModel(BsonDocument bson)
        {
            return new EntityResponseModel
            {
                id = bson.GetValueOrDefault("_id", Guid.Empty),
                type_id = bson.GetValueOrDefault("type_id", Guid.Empty),
                repository_id = bson.GetValueOrDefault("repository_id", Guid.Empty),
                status_id = bson.GetValueOrDefault("status_id", Guid.Empty),
                entity_code = bson.GetValueOrDefault("entity_code", string.Empty),
                entity_name = bson.GetValueOrDefault("entity_name", string.Empty),
                typeEntityName = bson.GetNestedValueOrDefault("CatalogData", "catalog_name"),
                repository_name = bson.GetNestedValueOrDefault("RepositoryData", "repository_databaseName"),
            };
        }


        public async Task<long> GetTotalRows(ISpecification<EntitiesEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }
        
        public static string GetPropertyName<T>(Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression member)
            {
                return member.Member.Name;
            }

            if (expression.Body is UnaryExpression unaryExpression && unaryExpression.Operand is MemberExpression memberExpr)
            {
                return memberExpr.Member.Name;
            }

            throw new ArgumentException("Invalid expression");
        }

    }
}
