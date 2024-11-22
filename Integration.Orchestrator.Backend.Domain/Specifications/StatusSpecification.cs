using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class StatusSpecification : ISpecification<StatusEntity>
    {
        public Expression<Func<StatusEntity, bool>> Criteria { get; private set; }

        public List<LookupSpecification<StatusEntity>> Includes { get; } = [];
        public Expression<Func<StatusEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<StatusEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public StatusSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
            SetupIncludes();
        }

        private static readonly Dictionary<string, Expression<Func<StatusEntity, object>>> sortExpressions = new()
            {
            { nameof(StatusEntity.status_key).Split("_")[1], x => x.status_key },
            { nameof(StatusEntity.status_text).Split("_")[1], x => x.status_text },
            { nameof(StatusEntity.status_color).Split("_")[1], x => x.status_color },
            { nameof(StatusEntity.status_background).Split("_")[1], x => x.status_background },
            { nameof(StatusEntity.updated_at).Split("_")[0], x => x.updated_at },
            { nameof(StatusEntity.created_at).Split("_")[0], x => x.created_at }
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
        private Expression<Func<StatusEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<StatusEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<StatusEntity, bool>> AddSearchCriteria(Expression<Func<StatusEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.status_text.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        private void SetupIncludes()
        {
          
        }
        public static Expression<Func<StatusEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<StatusEntity>.GetByUuid(x => x.id, id);
        }
        public static Expression<Func<StatusEntity, bool>> GetStatusIsActive(Guid id, string code)
        {
            return x => x.id == id && x.status_key == code;
        } 
        public static Expression<Func<StatusEntity, bool>> GetIdActiveStatus(string code)
        {
            return x =>x.status_key == code;
        }

        public static Expression<Func<StatusEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.status_key == code;
        }
    }
}
