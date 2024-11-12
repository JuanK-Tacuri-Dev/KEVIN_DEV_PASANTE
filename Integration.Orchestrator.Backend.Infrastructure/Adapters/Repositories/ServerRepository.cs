using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Server;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Infrastructure.Adapters.Repositories
{
    [ExcludeFromCodeCoverage]
    [Repository]
    public class ServerRepository(IMongoCollection<ServerEntity> collection) : IServerRepository<ServerEntity>
    {
        private readonly IMongoCollection<ServerEntity> _collection = collection;

        public Task InsertAsync(ServerEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(ServerEntity entity)
        {
            var filter = Builders<ServerEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<ServerEntity>.Update
                .Set(m => m.server_name, entity.server_name)
                .Set(m => m.type_id, entity.type_id)
                .Set(m => m.server_url, entity.server_url)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(ServerEntity entity)
        {
            var filter = Builders<ServerEntity>.Filter.Eq("_id", entity.id);
            return _collection.DeleteOneAsync(filter);
        }

        public async Task<ServerEntity> GetByIdAsync(Expression<Func<ServerEntity, bool>> specification)
        {
            var filter = Builders<ServerEntity>.Filter.Where(specification);
            var serverEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return serverEntity;
        }

        public async Task<ServerEntity> GetByCodeAsync(Expression<Func<ServerEntity, bool>> specification)
        {
            var filter = Builders<ServerEntity>.Filter.Where(specification);
            var serverEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return serverEntity;
        }

        public async Task<IEnumerable<ServerEntity>> GetByTypeAsync(Expression<Func<ServerEntity, bool>> specification)
        {
            var filter = Builders<ServerEntity>.Filter.Where(specification);
            var serverEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return serverEntity;
        }

        public async Task<IEnumerable<ServerEntity>> GetAllAsyncOld(ISpecification<ServerEntity> specification)
        {
            var filter = Builders<ServerEntity>.Filter.Where(specification.Criteria);

            var query = _collection
                .Find(filter)
                .Sort(specification.OrderBy != null
                    ? Builders<ServerEntity>.Sort.Ascending(specification.OrderBy)
                    : Builders<ServerEntity>.Sort.Descending(specification.OrderByDescending));

            if (specification.Skip >= 0)
            {
                query = query
                    .Limit(specification.Limit)
                    .Skip(specification.Skip);
            }
            return await query.ToListAsync();
        }


        public async Task<IEnumerable<ServerResponseModel>> GetAllAsync(ISpecification<ServerEntity> specification)
        {
            var filterBuilder = Builders<ServerEntity>.Filter;

            // Filtro principal según la especificación
            var filter = filterBuilder.Where(specification.Criteria);

            // Configuración de la consulta de agregación
            var query = _collection.Aggregate()
                .Match(filter)
                .Lookup(
                    foreignCollectionName: "Integration_Catalog",
                    localField: "type_id",
                    foreignField: "_id",
                    @as: "CatalogData"
                )
                .Lookup(
                    foreignCollectionName: "Integration_Status",
                    localField: "status_id",
                    foreignField: "_id",
                    @as: "StatusData"
                )
                .Unwind<BsonDocument>("CatalogData", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true });

            // Definición de ordenamiento
            var sortDefinitionBuilder = Builders<BsonDocument>.Sort;
            SortDefinition<BsonDocument> sortDefinition = sortDefinitionBuilder.Ascending("updated_at");

            string? orderByField = specification.OrderBy != null ? GetPropertyName(specification.OrderBy) :
                                  specification.OrderByDescending != null ? GetPropertyName(specification.OrderByDescending) : null;

            if (orderByField == "type_id")
                sortDefinition = specification.OrderBy != null ? sortDefinitionBuilder.Ascending("CatalogData.catalog_name") :
                                                                 sortDefinitionBuilder.Descending("CatalogData.catalog_name");
            else if (orderByField == "status_id")
                sortDefinition = specification.OrderBy != null ? sortDefinitionBuilder.Ascending("StatusData.status_text") :
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
                query = query.Skip(specification.Skip).Limit(specification.Limit);

            // Proyección
            var projection = Builders<BsonDocument>.Projection
                .Include("_id")
                .Include("server_code")
                .Include("server_name")
                .Include("type_id")
                .Include("server_url")
                .Include("status_id")
                .Include("CatalogData.catalog_name")
                .Include("StatusData.status_text");

            // Ejecutar y proyectar a `ServerResponseModel`
            var result = await query.Project<BsonDocument>(projection).ToListAsync();
            return result.Select(bson => new ServerResponseModel
            {
                id = bson["_id"].AsGuid,
                server_code = bson["server_code"].AsString,
                server_name = bson["server_name"].AsString,
                type_id = bson["type_id"].IsBsonNull ? (Guid?)null : bson["type_id"].AsGuid,
                server_url = bson["server_url"].AsString,
                status_id = bson["status_id"].AsGuid,
                type_name = bson.Contains("CatalogData") && bson["CatalogData"].IsBsonDocument
                            ? bson["CatalogData"]["catalog_name"].AsString : null,
                status_name = bson.Contains("StatusData") && bson["StatusData"].IsBsonArray
                            ? bson["StatusData"].AsBsonArray.FirstOrDefault()?.AsBsonDocument?.GetValue("status_text", BsonNull.Value).AsString : null
            });
        }

        public async Task<long> GetTotalRows(ISpecification<ServerEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

        public async Task<bool> ValidateNameURL(ServerEntity entity)
        {
            var filter = Builders<ServerEntity>.Filter.And(
                Builders<ServerEntity>.Filter.Eq(e => e.server_name, entity.server_name),
                Builders<ServerEntity>.Filter.Eq(e => e.server_url, entity.server_url),
                Builders<ServerEntity>.Filter.Ne(e => e.id, entity.id)
            );

            var count = await _collection.Find(filter).CountDocumentsAsync();
            return count >= 1;
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
