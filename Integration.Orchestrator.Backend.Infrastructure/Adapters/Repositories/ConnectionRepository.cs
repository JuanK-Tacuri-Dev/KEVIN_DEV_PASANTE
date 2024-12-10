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
    public class ConnectionRepository(IMongoCollection<ConnectionEntity> collection)
        : IConnectionRepository<ConnectionEntity>
    {
        private readonly IMongoCollection<ConnectionEntity> _collection = collection;
        private Dictionary<string, string> SortMapping = new()
            {
                 { "server_id", "ServerData.server_url" },
                { "adapter_id", "AdapterData.adapter_name" },
                { "repository_id", "RepositoryData.repository_databaseName" },
            };
        public Task InsertAsync(ConnectionEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(ConnectionEntity entity)
        {
            var filter = Builders<ConnectionEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<ConnectionEntity>.Update
                .Set(m => m.server_id, entity.server_id)
                .Set(m => m.adapter_id, entity.adapter_id)
                .Set(m => m.repository_id, entity.repository_id)
                .Set(m => m.connection_name, entity.connection_name)
                .Set(m => m.connection_description, entity.connection_description)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public Task DeleteAsync(ConnectionEntity entity)
        {
            var filter = Builders<ConnectionEntity>.Filter.Eq("_id", entity.id);
            return _collection.DeleteOneAsync(filter);
        }

        public async Task<ConnectionEntity> GetByExpressionIdAsync(Expression<Func<ConnectionEntity, bool>> specification)
        {
            var filter = Builders<ConnectionEntity>.Filter.Where(specification);
            var connectionEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return connectionEntity;
        }
        public async Task<ConnectionEntity> GetByIdAsync(Expression<Func<ConnectionEntity, bool>> specification)
        {
            var filter = Builders<ConnectionEntity>.Filter.Where(specification);
            var connectionEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return connectionEntity;
        }

        public async Task<ConnectionEntity> GetByCodeAsync(Expression<Func<ConnectionEntity, bool>> specification)
        {
            var filter = Builders<ConnectionEntity>.Filter.Where(specification);
            var connectionEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return connectionEntity;
        }

        public async Task<IEnumerable<ConnectionEntity>> GetByTypeAsync(Expression<Func<ConnectionEntity, bool>> specification)
        {
            var filter = Builders<ConnectionEntity>.Filter.Where(specification);
            var connectionEntity = await _collection
                .Find(filter)
                .ToListAsync();
            return connectionEntity;
        }

        public async Task<IEnumerable<ConnectionResponseModel>> GetAllAsync(ISpecification<ConnectionEntity> specification)
        {
            var filterBuilder = Builders<ConnectionEntity>.Filter;

            // Construir filtro principal desde la especificación
            var filter = filterBuilder.Where(specification.Criteria);

            // Configurar collation
            var collation = new Collation("en", strength: CollationStrength.Secondary);

            // Inicializar el pipeline de agregación con filtro y unwind
            var aggregation = _collection.Aggregate(new AggregateOptions { Collation = collation })
                                         .Match(filter)
                                         .Unwind("ServerData", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true })
                                         .Unwind("AdapterData", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true })
                                         .Unwind("RepositoryData", new AggregateUnwindOptions<BsonDocument> { PreserveNullAndEmptyArrays = true });

            // Obtener el campo de ordenamiento según la especificación
            string? orderByField = specification.OrderBy != null
                ? SortExpressionConfiguration<ConnectionEntity>.GetPropertyName(specification.OrderBy)
                : specification.OrderByDescending != null
                    ? SortExpressionConfiguration<ConnectionEntity>.GetPropertyName(specification.OrderByDescending)
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

        public async Task<long> GetTotalRows(ISpecification<ConnectionEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }
        #region Metodos Privados
      
        private ConnectionResponseModel MapToResponseModel(BsonDocument bson)
        {
            return new ConnectionResponseModel
            {
                id = bson.GetValueOrDefault("_id", Guid.Empty),
                server_id = bson.GetValueOrDefault("server_id", Guid.Empty),
                adapter_id = bson.GetValueOrDefault("adapter_id", Guid.Empty),
                repository_id = bson.GetValueOrDefault("repository_id", Guid.Empty),
                status_id = bson.GetValueOrDefault("status_id", Guid.Empty),
                connection_code = bson.GetValueOrDefault("connection_code", string.Empty),
                connection_name = bson.GetValueOrDefault("connection_name", string.Empty),
                connection_description = bson.GetValueOrDefault("connection_description", string.Empty),
                adapterName = bson.GetNestedValueOrDefault("AdapterData", "adapter_name"),
                repositoryName = bson.GetNestedValueOrDefault("RepositoryData", "repository_databaseName"),
                serverName = bson.GetNestedValueOrDefault("ServerData", "server_name"),
                serverUrl = bson.GetNestedValueOrDefault("ServerData", "server_url")
            };

        }

        #endregion


    }
}
