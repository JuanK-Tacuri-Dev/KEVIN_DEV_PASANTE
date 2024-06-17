using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class SynchronizationStatesSpecification : ISpecification<SynchronizationStatesEntity>
    {
        public Expression<Func<SynchronizationStatesEntity, bool>> Criteria { get; private set; }

        public Expression<Func<SynchronizationStatesEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<SynchronizationStatesEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public SynchronizationStatesSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<SynchronizationStatesEntity, object>>> sortExpressions 
            = new Dictionary<string, Expression<Func<SynchronizationStatesEntity, object>>>
        {
            { nameof(SynchronizationStatesEntity.name), x => x.name },
            { nameof(SynchronizationStatesEntity.code), x => x.code },
            { nameof(SynchronizationStatesEntity.color), x => x.color },
            { nameof(SynchronizationStatesEntity.updated_at), x => x.updated_at },
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
        private Expression<Func<SynchronizationStatesEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<SynchronizationStatesEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<SynchronizationStatesEntity, bool>> AddSearchCriteria(Expression<Func<SynchronizationStatesEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.name.ToUpper().Contains(search.ToUpper()) ||
                x.code.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<SynchronizationStatesEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<SynchronizationStatesEntity>.GetByUuid(x => x.id, id);
        }

    }
}
