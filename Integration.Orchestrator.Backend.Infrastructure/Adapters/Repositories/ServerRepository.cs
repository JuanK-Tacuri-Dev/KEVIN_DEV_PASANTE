using Integration.Orchestrator.Backend.Domain.Dto.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Server;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Specifications;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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


        public async Task<IEnumerable<ServerDto>> GetAllAsyncold2(ISpecification<ServerDto> specification)
        {
            var pipeline = new List<BsonDocument>();

            // 1. Filtro inicial basado en ServerDto
            if (specification.Criteria != null)
            {
                var filter = new BsonDocument("$match", new BsonDocument("$expr", specification.Criteria.ToBsonDocument()));
                pipeline.Add(filter);
            }

            // 2. LEFT JOIN con la colección "Integration_Catalog"
            pipeline.Add(new BsonDocument("$lookup", new BsonDocument
             {
                 { "from", "Integration_Catalog" },
                 { "localField", "type_id" },
                 { "foreignField", "id" },
                 { "as", "catalogo" }
             }));

            // 3. Proyección para obtener ServerDto
            pipeline.Add(new BsonDocument("$project", new BsonDocument
             {
                 { "Id", "$_id" },
                 { "ServerName", "$name" },
                 { "type_id", "$type_id" },
                 { "typeServerName", new BsonDocument("$arrayElemAt", new BsonArray { "$catalogo.catalog_name", 0 }) }
             }));

            // 4. Ordenamiento
            if (specification.OrderBy != null)
            {
                var orderByProperty = GetPropertyName(specification.OrderBy);
                pipeline.Add(new BsonDocument("$sort", new BsonDocument(orderByProperty, 1)));  // Ascendente
            }
            else if (specification.OrderByDescending != null)
            {
                var orderByProperty = GetPropertyName(specification.OrderByDescending);
                pipeline.Add(new BsonDocument("$sort", new BsonDocument(orderByProperty, -1)));  // Descendente
            }

            // 5. Paginación
            if (specification.Skip > 0)
            {
                pipeline.Add(new BsonDocument("$skip", specification.Skip));
            }
            if (specification.Limit > 0)
            {
                pipeline.Add(new BsonDocument("$limit", specification.Limit));
            }

            // Ejecución del pipeline
            var results = await _collection.Aggregate<BsonDocument>(pipeline).ToListAsync();

            // Mapeo a ServerDto
            //var serverDtos = results.Select(result =>
            //{
            //    var serverDto = new ServerDto
            //    {
            //        Id = result["Id"].AsObjectId,
            //        ServerName = result["ServerName"].AsString,
            //        type_id = result["type_id"].AsObjectId,
            //        typeServerName = result["typeServerName"]?.AsString
            //    };
            //    return serverDto;
            //});

            return null;
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




        public async Task<IEnumerable<ServerDto>> GetAllAsync(ISpecification<ServerEntity> specification)
        {


            var filter = Builders<ServerEntity>.Filter.Where(specification.Criteria);


            var aggregation = _collection.Aggregate().Match(filter).Lookup<CatalogEntity, BsonDocument>("Integration_Catalog", "type_id", "id", "catalogo");


            var results = await aggregation.ToListAsync();

            var serverDtos = results.Select(result =>
            {
                var server = BsonSerializer.Deserialize<ServerDto>(result);

                var catalogEntities = result["catalogo"].AsBsonArray;

                server.TypeServerName = catalogEntities.Count > 0
                    ? catalogEntities[0]["catalog_name"].AsString
                    : null;

                return server;
            }).AsQueryable();


            IEnumerable<ServerDto> OrderedServerDtos;
            if (specification.OrderBy != null)
            {
                var orderExpression = SortExpressionConfiguration<ServerDto>.ConvertOrderExpression(specification.OrderBy);
                OrderedServerDtos = serverDtos.OrderBy(orderExpression);
            }
            else if (specification.OrderByDescending != null)
            {
                var orderExpression = SortExpressionConfiguration<ServerDto>.ConvertOrderExpression(specification.OrderByDescending);
                OrderedServerDtos = serverDtos.OrderByDescending(orderExpression);
            }
            else
            {
                OrderedServerDtos = serverDtos;
            }

            if (specification.Skip > 0)
            {
                OrderedServerDtos = OrderedServerDtos.Skip(specification.Skip);
            }

            if (specification.Limit > 0)
            {
                OrderedServerDtos = OrderedServerDtos.Take(specification.Limit);
            }

            return OrderedServerDtos;

        }

           public async Task<IEnumerable<ServerResponseTest>> GetAllAsyncTest(ISpecification<ServerEntity> specification)
        {


            var filter = Builders<ServerEntity>.Filter.Where(specification.Criteria);


            var aggregation = _collection.Aggregate().Match(filter).Lookup<CatalogEntity, BsonDocument>("Integration_Catalog", "type_id", "id", "catalogo");


            var results = await aggregation.ToListAsync();

            var serverDtos = results.Select(result =>
            {
                var server = BsonSerializer.Deserialize<ServerResponseTest>(result);

                var catalogEntities = result["catalogo"].AsBsonArray;

                server.TypeServerName = catalogEntities.Count > 0
                    ? catalogEntities[0]["catalog_name"].AsString
                    : null;

                return server;
            }).AsQueryable();


            IEnumerable<ServerResponseTest> OrderedServerDtos;
            if (specification.OrderBy != null)
            {
                var orderExpression = SortExpressionConfiguration<ServerResponseTest>.ConvertOrderExpression(specification.OrderBy);
                OrderedServerDtos = serverDtos.OrderBy(orderExpression);
            }
            else if (specification.OrderByDescending != null)
            {
                var orderExpression = SortExpressionConfiguration<ServerResponseTest>.ConvertOrderExpression(specification.OrderByDescending);
                OrderedServerDtos = serverDtos.OrderByDescending(orderExpression);
            }
            else
            {
                OrderedServerDtos = serverDtos;
            }

            if (specification.Skip > 0)
            {
                OrderedServerDtos = OrderedServerDtos.Skip(specification.Skip);
            }

            if (specification.Limit > 0)
            {
                OrderedServerDtos = OrderedServerDtos.Take(specification.Limit);
            }

            return OrderedServerDtos;

        }


        //public async Task<IEnumerable<ServerEntity>> GetAllAsync(ISpecification<ServerEntity> specification)
        //{
        //    var filter = Builders<ServerEntity>.Filter.Where(specification.Criteria);

        //    var query = _collection
        //        .Find(filter)
        //        .Sort(specification.OrderBy != null
        //            ? Builders<ServerEntity>.Sort.Ascending(specification.OrderBy)
        //            : Builders<ServerEntity>.Sort.Descending(specification.OrderByDescending));

        //    if (specification.Skip >= 0)
        //    {
        //        query = query
        //            .Limit(specification.Limit)
        //            .Skip(specification.Skip);
        //    }
        //    return await query.ToListAsync();
        //}

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
    }
}
