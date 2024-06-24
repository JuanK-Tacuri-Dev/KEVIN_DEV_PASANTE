using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class ConnectionSpecification : ISpecification<ConnectionEntity>
    {
        public Expression<Func<ConnectionEntity, bool>> Criteria { get; private set; }

        public Expression<Func<ConnectionEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<ConnectionEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public ConnectionSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<ConnectionEntity, object>>> sortExpressions 
            = new Dictionary<string, Expression<Func<ConnectionEntity, object>>>
        {
            { nameof(ConnectionEntity.connection_code), x => x.connection_code },
            { nameof(ConnectionEntity.server), x => x.server },
            { nameof(ConnectionEntity.adapter), x => x.adapter }
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
        private Expression<Func<ConnectionEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<ConnectionEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<ConnectionEntity, bool>> AddSearchCriteria(Expression<Func<ConnectionEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.connection_code.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<ConnectionEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.connection_code == code;
        }

        public static Expression<Func<ConnectionEntity, bool>> GetByTypeExpression(string type)
        {
            return x => true && x.adapter == type;
        }


    }
}
