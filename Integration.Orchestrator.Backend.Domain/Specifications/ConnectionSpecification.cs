using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;

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
            { nameof(ConnectionEntity.connection_code).Split("_")[1], x => x.connection_code },
            { nameof(ConnectionEntity.connection_name).Split("_")[1], x => x.connection_name },
            { nameof(ConnectionEntity.connection_description).Split("_")[1], x => x.connection_description },
            { nameof(ConnectionEntity.created_at).Split("_")[0], x => x.created_at }
        };
        private void SetupPagination(PaginatedModel model)
        {
            Skip = (model.First - 1) * model.Rows;
            Limit = model.Rows;
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

        public static Expression<Func<ConnectionEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.connection_code == code;
        }
    }
}
