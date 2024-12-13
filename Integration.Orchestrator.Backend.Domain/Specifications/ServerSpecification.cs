using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models;


namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class ServerSpecification : ISpecification<ServerEntity>
    {
        public Expression<Func<ServerEntity, bool>> Criteria { get; set; }

        public Expression<Func<ServerEntity, object>> OrderBy { get; private set; }
        public List<LookupSpecification<ServerEntity>> Includes { get; } = [];

        public Expression<Func<ServerEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }

        public ServerSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
            SetupIncludes();
        }

        private static readonly Dictionary<string, Expression<Func<ServerEntity, object>>> sortExpressions = new()
        {
            { Utilities.GetSafeKey(nameof(ServerEntity.server_code), 1), x => x.server_code },
            { Utilities.GetSafeKey(nameof(ServerEntity.server_name), 1), x => x.server_name },
            { Utilities.GetSafeKey(nameof(ServerEntity.server_url), 1), x => x.server_url },
            { "typeServerName", x => x.type_id },
            { "status", x => x.status_id },
            { Utilities.GetSafeKey(nameof(ServerEntity.updated_at), 0), x => x.updated_at },
            { Utilities.GetSafeKey(nameof(ServerEntity.created_at), 0), x => x.created_at }
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
        private Expression<Func<ServerEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<ServerEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private void SetupIncludes()
        {
            Includes.Add(new LookupSpecification<ServerEntity> { Collection = "Integration_Catalog", LocalField = "type_id", ForeignField = "_id", As = "CatalogData" });
        }

        private Expression<Func<ServerEntity, bool>> AddSearchCriteria(Expression<Func<ServerEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.server_url.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<ServerEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<ServerEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<ServerEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.server_code == code;
        }

        public static Expression<Func<ServerEntity, bool>> GetByTypeExpression(Guid typeId)
        {
            return x => true && x.type_id == typeId;
        }


    }
}
