using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Entity;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Server;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [ExcludeFromCodeCoverage]
    [Repository]
    public class EntitiesRepository(IMongoCollection<EntitiesEntity> collection)
        : IEntitiesRepository<EntitiesEntity>
    {
        private readonly IMongoCollection<EntitiesEntity> _collection = collection;

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

            /*  var query = _collection
                  .Find(filter)
                  .Sort(specification.OrderBy != null
                      ? Builders<EntitiesEntity>.Sort.Ascending(specification.OrderBy)
                      : Builders<EntitiesEntity>.Sort.Descending(specification.OrderByDescending));
*/
            var query = _collection.Aggregate()
                 .Match(filter)
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
                 )
                 .Unwind("CatalogData", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true })
                 .Unwind("RepositoryData", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true });

            // Definición de ordenamiento
            var sortDefinitionBuilder = Builders<BsonDocument>.Sort;
            SortDefinition<BsonDocument> sortDefinition = sortDefinitionBuilder.Ascending("updated_at");

            string? orderByField = specification.OrderBy != null ? GetPropertyName(specification.OrderBy) :
                                  specification.OrderByDescending != null ? GetPropertyName(specification.OrderByDescending) : null;

            if (orderByField == "type_id")
                sortDefinition = specification.OrderBy != null ? sortDefinitionBuilder.Ascending("CatalogData.catalog_name") :
                                                               sortDefinitionBuilder.Descending("StatusData.status_text");
            else if (orderByField != null)
            {
                // Ordenamiento para cualquier otro campo que no sea UUID
                sortDefinition = specification.OrderBy != null ? sortDefinitionBuilder.Ascending(orderByField) :
                                                                 sortDefinitionBuilder.Descending(orderByField);
            }
            else
            {
                // Ordenamiento por defecto si no se especifica ningún campo
                sortDefinition = sortDefinitionBuilder.Ascending("updated_at");
            }


            // Aplicamos el ordenamiento y la paginación
            query = query.Sort(sortDefinition);
            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            // Proyección
            var projection = Builders<BsonDocument>.Projection
                .Include("_id")
                .Include("entity_code")
                .Include("entity_name")
                .Include("type_id")
                .Include("repository_id")
                .Include("status_id")
                .Include("CatalogData.catalog_name")
                .Include("RepositoryData.repository_databaseName");

            
            var result = await query.Project<BsonDocument>(projection).ToListAsync();
            return result.Select(bson => new EntityResponseModel
            {
                id = bson["_id"].AsGuid,
                entity_code = bson["entity_code"].AsString,
                entity_name = bson["entity_name"].AsString,
                type_id = bson["type_id"].AsGuid,
                repository_id = bson["repository_id"].AsGuid,
                status_id = bson["status_id"].AsGuid,
                typeEntityName = bson.Contains("CatalogData") && bson["CatalogData"].IsBsonDocument
                            ? bson["CatalogData"]["catalog_name"].AsString : null,
                repository_name = bson.Contains("RepositoryData") && bson["RepositoryData"].IsBsonDocument
                            ? bson["RepositoryData"]["repository_databaseName"].AsString : null
            });
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
