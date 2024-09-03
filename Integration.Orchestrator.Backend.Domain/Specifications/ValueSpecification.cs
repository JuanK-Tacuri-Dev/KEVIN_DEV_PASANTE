using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class ValueSpecification : ISpecification<ValueEntity>
    {
        public Expression<Func<ValueEntity, bool>> Criteria { get; private set; }

        public Expression<Func<ValueEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<ValueEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public ValueSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<ValueEntity, object>>> sortExpressions 
            = new Dictionary<string, Expression<Func<ValueEntity, object>>>
        {
            { nameof(ValueEntity.value_type), x => x.value_type },
            { nameof(ValueEntity.value_code), x => x.value_code }
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
        private Expression<Func<ValueEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<ValueEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<ValueEntity, bool>> AddSearchCriteria(Expression<Func<ValueEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.value_code.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<ValueEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<ValueEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<ValueEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.value_code == code;
        }

        public static Expression<Func<ValueEntity, bool>> GetByTypeExpression(string type)
        {
            return x => true && x.value_type == type;
        }


    }
}
