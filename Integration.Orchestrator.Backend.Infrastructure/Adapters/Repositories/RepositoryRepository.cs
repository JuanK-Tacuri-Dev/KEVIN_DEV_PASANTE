using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models.Configurador;
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
    public class RepositoryRepository(IMongoCollection<RepositoryEntity> collection) 
        : IRepositoryRepository<RepositoryEntity>
    {
        private readonly IMongoCollection<RepositoryEntity> _collection = collection;
        
        public Task InsertAsync(RepositoryEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(RepositoryEntity entity)
        {
            var filter = Builders<RepositoryEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<RepositoryEntity>.Update
                .Set(m => m.repository_port, entity.repository_port)
                .Set(m => m.repository_userName, entity.repository_userName)
                .Set(m => m.repository_password, entity.repository_password)
                .Set(m => m.repository_databaseName, entity.repository_databaseName)
                .Set(m => m.auth_type_id, entity.auth_type_id)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(RepositoryEntity entity)
        {
            var filter = Builders<RepositoryEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<RepositoryEntity> GetByIdAsync(Expression<Func<RepositoryEntity, bool>> specification)
        {
            var filter = Builders<RepositoryEntity>.Filter.Where(specification);
            return await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<RepositoryEntity> GetByCodeAsync(Expression<Func<RepositoryEntity, bool>> specification)
        {
            var filter = Builders<RepositoryEntity>.Filter.Where(specification);
            return await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<RepositoryResponseModel>> GetAllAsync(ISpecification<RepositoryEntity> specification)
        {
            var filterBuilder = Builders<RepositoryEntity>.Filter;

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
            var sortDefinition = GetSortDefinition(orderByField, specification.OrderBy != null);

            // Aplicar joins si hay especificaciones de include
            if (specification.Includes != null)
            {
                foreach (var join in specification.Includes)
                {
                    aggregation = aggregation.Lookup(join.Collection, join.LocalField, join.ForeignField, join.As);
                }
            }

            aggregation = aggregation.Sort(sortDefinition);

            // Configurar proyección
            var projection = Builders<BsonDocument>.Projection
                .Include("_id")
                .Include("auth_type_id")
                .Include("status_id")
                .Include("repository_port")
                .Include("repository_code")
                .Include("repository_databaseName")
                .Include("repository_password")
                .Include("repository_userName")
                .Include("CatalogData.catalog_name");

            // Ejecutar agregación y obtener resultados
            var result = await aggregation.Project<BsonDocument>(projection).ToListAsync();

            // Mapear resultados a ServerResponseModel
            var data = result.Select(MapToResponseModel);

            return data;
        }

        //public async Task<IEnumerable<RepositoryEntity>> GetAllAsync(ISpecification<RepositoryEntity> specification)
        //{
        //    var filter = Builders<RepositoryEntity>.Filter.Where(specification.Criteria);

        //    var query = _collection
        //        .Find(filter)
        //        .Sort(specification.OrderBy != null
        //            ? Builders<RepositoryEntity>.Sort.Ascending(specification.OrderBy)
        //            : Builders<RepositoryEntity>.Sort.Descending(specification.OrderByDescending));

        //    if (specification.Skip >= 0)
        //    {
        //        query = query
        //            .Limit(specification.Limit)
        //            .Skip(specification.Skip);
        //    }
        //    return await query.ToListAsync();
        //}

        public async Task<long> GetTotalRows(ISpecification<RepositoryEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

        public async Task<bool> ValidateDbPortUser(RepositoryEntity entity)
        {
            var filter = Builders<RepositoryEntity>.Filter.And(
                Builders<RepositoryEntity>.Filter.Eq(e => e.repository_databaseName, entity.repository_databaseName),
                Builders<RepositoryEntity>.Filter.Eq(e => e.repository_port, entity.repository_port),
                Builders<RepositoryEntity>.Filter.Eq(e => e.repository_userName, entity.repository_userName),
                Builders<RepositoryEntity>.Filter.Ne(e => e.id, entity.id)
            );

            var count = await _collection.Find(filter).CountDocumentsAsync();
            return count >= 1;
        }


        #region Metodos Privados
        private SortDefinition<BsonDocument> GetSortDefinition(string? orderByField, bool isAscending)
        {
            var sortDefinitionBuilder = Builders<BsonDocument>.Sort;

            // Diccionario para mapear campos de ordenamiento específicos
            var sortMapping = new Dictionary<string, string>
            {
                { "auth_type_id", "CatalogData.catalog_name" }
            };

            // Si no se especifica un campo, usar el predeterminado
            if (orderByField == null)
            {
                return sortDefinitionBuilder.Ascending("updated_at");
            }

            // Intentar obtener el campo correspondiente del diccionario
            var sortField = sortMapping.ContainsKey(orderByField) ? sortMapping[orderByField] : orderByField;

            // Retornar la definición de orden
            return isAscending
                ? sortDefinitionBuilder.Ascending(sortField)
                : sortDefinitionBuilder.Descending(sortField);
        }
        private RepositoryResponseModel MapToResponseModel(BsonDocument bson)
        {
            return new RepositoryResponseModel
            {
                id = bson.GetValueOrDefault("_id", Guid.Empty),
                auth_type_id = bson.GetValueOrDefault("auth_type_id", Guid.Empty),
                status_id = bson.GetValueOrDefault("status_id", Guid.Empty),
                repository_port = bson.GetValueOrDefault("repository_port", 0),
                repository_code = bson.GetValueOrDefault("repository_code", string.Empty),
                repository_databaseName = bson.GetValueOrDefault("repository_databaseName", string.Empty),
                repository_password = bson.GetValueOrDefault("repository_password", string.Empty),
                repository_userName = bson.GetValueOrDefault("repository_userName", string.Empty),
                authTypeName = bson.GetNestedValueOrDefault("CatalogData", "catalog_name")
            };

        }

        #endregion

    }
}
