using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class IntegrationSpecification : ISpecification<IntegrationEntity>
    {
        public Expression<Func<IntegrationEntity, bool>> Criteria { get; private set; }

        public Expression<Func<IntegrationEntity, object>> OrderBy { get; private set; }

        public Expression<Func<IntegrationEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }

        public IntegrationSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<IntegrationEntity, object>>> sortExpressions
            = new Dictionary<string, Expression<Func<IntegrationEntity, object>>>
        {
            { nameof(IntegrationEntity.name), x => x.name },
            { nameof(IntegrationEntity.status), x => x.status },
            { nameof(IntegrationEntity.observations), x => x.observations },
            { nameof(IntegrationEntity.user_id), x => x.user_id },
            { nameof(IntegrationEntity.updated_at), x => x.updated_at },
        };
        private void SetupPagination(PaginatedModel model)
        {
            Skip = (model.Page - 1) * model.Rows;
            Limit = model.Rows;
        }

        private void SetupOrdering(PaginatedModel model)
        {
            if (sortExpressions.TryGetValue(model.SortBy, out var expression))
            {
                if (model.SortOrder == SortOrdering.Ascending)
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

        private Expression<Func<IntegrationEntity, bool>> AddSearchCriteria(Expression<Func<IntegrationEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.name.ToUpper().Contains(search.ToUpper()) ||
                x.observations.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<IntegrationEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<IntegrationEntity>.GetByUuid(x => x.id, id);
        }

    }
}
