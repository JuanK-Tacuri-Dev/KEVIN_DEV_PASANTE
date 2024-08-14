using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class CatalogSpecification : ISpecification<CatalogEntity>
    {
        public Expression<Func<CatalogEntity, bool>> Criteria { get; private set; }

        public Expression<Func<CatalogEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<CatalogEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public CatalogSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<CatalogEntity, object>>> sortExpressions 
            = new Dictionary<string, Expression<Func<CatalogEntity, object>>>
        {
            { nameof(CatalogEntity.name), x => x.name },
            { nameof(CatalogEntity.detail), x => x.detail },
            { nameof(CatalogEntity.value), x => x.value }
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
        private Expression<Func<CatalogEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<CatalogEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<CatalogEntity, bool>> AddSearchCriteria(Expression<Func<CatalogEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.name.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<CatalogEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<CatalogEntity>.GetByUuid(x => x.id, id);
        }


        public static Expression<Func<CatalogEntity, bool>> GetByFatherExpression(Guid fatherId)
        {
            return x => true && x.father_id == fatherId;
        }


    }
}
