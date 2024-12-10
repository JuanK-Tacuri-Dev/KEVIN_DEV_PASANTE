using Integration.Orchestrator.Backend.Domain.Commons;
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
    public class ServerRepository(IMongoCollection<ServerEntity> collection) : IServerRepository<ServerEntity>
    {
        private readonly IMongoCollection<ServerEntity> _collection = collection;



        private Dictionary<string, string> SortMapping = new()
            {
                { "type_id", "CatalogData.catalog_name" }
            };


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

            var data = result.Select(MapToResponseModel);

            return data;
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


        #region Metodos Privados
        // Método para mapear un BsonDocument a ServerResponseModel
        private ServerResponseModel MapToResponseModel(BsonDocument bson)
        {

            return new ServerResponseModel
            {
                id = bson.GetValueOrDefault("_id", Guid.Empty),
                status_id = bson.GetValueOrDefault("status_id", Guid.Empty),
                type_id = bson.GetValueOrDefault("type_id", Guid.Empty),
                server_code = bson.GetValueOrDefault("server_code", string.Empty),
                server_name = bson.GetValueOrDefault("server_name", string.Empty),
                server_url = bson.GetValueOrDefault("server_url", string.Empty),
                type_name = bson.GetNestedValueOrDefault("CatalogData", "catalog_name")
            };

        }




        #endregion

    }
}
