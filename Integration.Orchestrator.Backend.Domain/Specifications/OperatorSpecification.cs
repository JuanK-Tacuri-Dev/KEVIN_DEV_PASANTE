using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class OperatorSpecification : ISpecification<OperatorEntity>
    {
        public Expression<Func<OperatorEntity, bool>> Criteria { get; private set; }

        public Expression<Func<OperatorEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<OperatorEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public OperatorSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<OperatorEntity, object>>> sortExpressions 
            = new Dictionary<string, Expression<Func<OperatorEntity, object>>>
        {
            { nameof(OperatorEntity.type_id), x => x.type_id },
            { nameof(OperatorEntity.operator_code), x => x.operator_code }
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
        private Expression<Func<OperatorEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<OperatorEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<OperatorEntity, bool>> AddSearchCriteria(Expression<Func<OperatorEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.operator_code.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<OperatorEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<OperatorEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<OperatorEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.operator_code == code;
        }

        public static Expression<Func<OperatorEntity, bool>> GetByTypeExpression(Guid typeId)
        {
            return x => true && x.type_id == typeId;
        }


    }
}
