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
    public class IntegrationRepository(IMongoCollection<IntegrationEntity> collection, IMongoCollection<ProcessEntity> processCollection) 
        : IIntegrationRepository<IntegrationEntity>
    {
        private readonly IMongoCollection<IntegrationEntity> _collection = collection;
        private readonly IMongoCollection<ProcessEntity> _ProcessCollection = processCollection;


        public Task InsertAsync(IntegrationEntity entity)
        {
            return _collection.InsertOneAsync(entity);
        }

        public Task UpdateAsync(IntegrationEntity entity)
        {
            var filter = Builders<IntegrationEntity>.Filter.Eq("_id", entity.id);
            var update = Builders<IntegrationEntity>.Update
                .Set(m => m.integration_name, entity.integration_name)
                .Set(m => m.status_id, entity.status_id)
                .Set(m => m.integration_observations, entity.integration_observations)
                .Set(m => m.user_id, entity.user_id)
                .Set(m => m.process, entity.process)
                .Set(m => m.updated_at, entity.updated_at);
            return _collection.UpdateOneAsync(filter, update);
        }

        public async Task DeleteAsync(IntegrationEntity entity)
        {
            var filter = Builders<IntegrationEntity>.Filter.Eq("_id", entity.id);
            await _collection.DeleteOneAsync(filter);
        }

        public async Task<IntegrationEntity> GetByIdAsync(Expression<Func<IntegrationEntity, bool>> specification)
        {
            var filter = Builders<IntegrationEntity>.Filter.Where(specification);
            var integrationEntity = await _collection
                .Find(filter)
                .FirstOrDefaultAsync();
            return integrationEntity;
        }

            public async Task<IEnumerable<IntegrationResponseModel>> GetAllAsync(ISpecification<IntegrationEntity> specification)
            {
                var filterBuilder = Builders<IntegrationEntity>.Filter;

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
                    .Include("user_id")
                    .Include("status_id")
                    .Include("integration_name")
                    .Include("process")
                    .Include("integration_observations");

                // Ejecutar agregación y obtener resultados
                var result = await aggregation.Project<BsonDocument>(projection).ToListAsync();

                // Mapear resultados a IntegrationResponseModel, obteniendo los nombres de process
                var data = new List<IntegrationResponseModel>();

                foreach (var bson in result)
                {
                    var model = await MapToResponseModel(bson);
                    data.Add(model);
                }

                return data;
            }
        public async Task<long> GetTotalRows(ISpecification<IntegrationEntity> specification)
        {
            return await _collection
                .Find(specification.Criteria)
                .CountDocumentsAsync();
        }

        #region Metodos Privados
        private SortDefinition<BsonDocument> GetSortDefinition(string? orderByField, bool isAscending)
        {
            var sortDefinitionBuilder = Builders<BsonDocument>.Sort;

            // Diccionario para mapear campos de ordenamiento específicos
            var sortMapping = new Dictionary<string, string>
            {
                //{ "type_id", "CatalogData.catalog_name" }
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


        private async Task<IntegrationResponseModel> MapToResponseModel(BsonDocument bson)
        {

            var processIds = bson["process"].AsBsonArray.Select(p => p.AsGuid).ToList();
            
            var filter = Builders<ProcessEntity>.Filter.In("_id", processIds);

            var catalogoDocuments = await _ProcessCollection.Find(filter).ToListAsync();
            
            var processes = catalogoDocuments.Select(catalogoDoc => new IntegrationProcess
            {
                id = catalogoDoc.id, 
                name = catalogoDoc.process_name  
            }).ToList();

            return new IntegrationResponseModel
            {
                id = bson.GetValueOrDefault("_id", Guid.Empty),
                user_id = bson.GetValueOrDefault("user_id", Guid.Empty),
                status_id = bson.GetValueOrDefault("status_id", Guid.Empty),
                integration_name = bson.GetValueOrDefault("integration_name", string.Empty),
                integration_observations = bson.GetValueOrDefault("integration_observations", string.Empty),
                process = processes
            };

        }


        #endregion




    }
}
