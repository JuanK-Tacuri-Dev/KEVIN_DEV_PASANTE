using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class IntegrationSpecification : ISpecification<IntegrationEntity>
    {
        public Expression<Func<IntegrationEntity, bool>> Criteria { get; set; }
        public List<LookupSpecification<IntegrationEntity>> Includes { get; } = [];

        public Expression<Func<IntegrationEntity, object>> OrderBy { get; private set; }

        public Expression<Func<IntegrationEntity, object>> OrderByDescending { get; private set; }
        public Dictionary<string, object> AdditionalFilters { get; } = [];

        public int Skip { get; private set; }

        public int Limit { get; private set; }

        public IntegrationSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
            SetupIncludes();
        }

        private static readonly Dictionary<string, Expression<Func<IntegrationEntity, object>>> sortExpressions= new()
        {
            { nameof(IntegrationEntity.integration_name).Split("_")[1], x => x.integration_name },
            { nameof(IntegrationEntity.integration_observations).Split("_")[1], x => x.integration_observations },
            { "status", x => x.status_id },
            { "processName", x => x.process },
            { nameof(IntegrationEntity.updated_at).Split("_")[0], x => x.updated_at },
            { nameof(IntegrationEntity.created_at).Split("_")[0], x => x.created_at },
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
        private Expression<Func<IntegrationEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<IntegrationEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }
        private void SetupIncludes()
        {
            Includes.Add(new LookupSpecification<IntegrationEntity> { Collection = "Integration_Catalog", LocalField = "type_id", ForeignField = "_id", As = "CatalogData" });
        }
        private Expression<Func<IntegrationEntity, bool>> AddSearchCriteria(Expression<Func<IntegrationEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.integration_name.ToUpper().Contains(search.ToUpper()) ||
                x.integration_observations.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<IntegrationEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<IntegrationEntity>.GetByUuid(x => x.id, id);
        }
        public static Expression<Func<IntegrationEntity, bool>> GetByProcessIdExpression(Guid id, Guid idStatus)
        {
            return x => x.process.Contains(id) && x.status_id == idStatus;
        }

    }
}
