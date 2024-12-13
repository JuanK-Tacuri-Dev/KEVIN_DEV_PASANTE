using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class ConnectionSpecification : ISpecification<ConnectionEntity>
    {
        public Expression<Func<ConnectionEntity, bool>> Criteria { get; set; }
        public List<LookupSpecification<ConnectionEntity>> Includes { get; } = [];

        public Expression<Func<ConnectionEntity, object>> OrderBy { get; private set; }

        public Expression<Func<ConnectionEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }

        public ConnectionSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
            SetupIncludes();
        }

        private static readonly Dictionary<string, Expression<Func<ConnectionEntity, object>>> sortExpressions = new()
            {
            { nameof(ConnectionEntity.connection_code).Split("_")[1], x => x.connection_code },
            { nameof(ConnectionEntity.connection_name).Split("_")[1], x => x.connection_name },
            { nameof(ConnectionEntity.connection_description).Split("_")[1], x => x.connection_description },
            { "serverUrl", x => x.server_id },
            { "adapterName", x => x.adapter_id },
            { "repositoryName", x => x.repository_id },
            { "status", x => x.status_id },
            { nameof(ConnectionEntity.updated_at).Split("_")[0], x => x.updated_at },
            { nameof(ConnectionEntity.created_at).Split("_")[0], x => x.created_at }
        };
        private void SetupPagination(PaginatedModel model)
        {
            if (model.Rows > 0)
            {
                Skip = model.First;
                Limit = model.Rows;
            }
        }

        private void SetupOrdering(PaginatedModel model)
        {
            if (sortExpressions.TryGetValue(model.Sort_field, out var expression))
            {
                if (model.Sort_order == SortOrdering.Ascending)
                {
                    OrderBy = expression;
                }
                else
                {
                    OrderByDescending = expression;
                }
            }
            if (OrderBy == null && OrderByDescending == null)
            {
                OrderBy = (x => x.id);
            }
        }
        private Expression<Func<ConnectionEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<ConnectionEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private void SetupIncludes()
        {
            Includes.Add(new LookupSpecification<ConnectionEntity> { Collection = "Integration_Server", LocalField = "server_id", ForeignField = "_id", As = "ServerData" });
            Includes.Add(new LookupSpecification<ConnectionEntity> { Collection = "Integration_Adapter", LocalField = "adapter_id", ForeignField = "_id", As = "AdapterData" });
            Includes.Add(new LookupSpecification<ConnectionEntity> { Collection = "Integration_Repository", LocalField = "repository_id", ForeignField = "_id", As = "RepositoryData" });
        }
        private Expression<Func<ConnectionEntity, bool>> AddSearchCriteria(Expression<Func<ConnectionEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.connection_name.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<ConnectionEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<ConnectionEntity>.GetByUuid(x => x.id, id);
        }
        public static Expression<Func<ConnectionEntity, bool>> GetByServerIdExpression(Guid id, Guid idStatusActive) => x => x.server_id == id && x.status_id==idStatusActive;
        public static Expression<Func<ConnectionEntity, bool>> GetByAdapterIdExpression(Guid id, Guid idStatusActive) => x => x.adapter_id == id && x.status_id == idStatusActive;
        public static Expression<Func<ConnectionEntity, bool>> GetByRepositoryIdExpression(Guid id, Guid idStatusActive) => x => x.repository_id == id && x.status_id == idStatusActive;

        public static Expression<Func<ConnectionEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.connection_code == code;
        }
    }
}
