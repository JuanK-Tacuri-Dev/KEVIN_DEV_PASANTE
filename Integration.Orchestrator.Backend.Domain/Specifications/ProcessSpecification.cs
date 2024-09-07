using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class ProcessSpecification : ISpecification<ProcessEntity>
    {
        public Expression<Func<ProcessEntity, bool>> Criteria { get; private set; }

        public Expression<Func<ProcessEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<ProcessEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public ProcessSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<ProcessEntity, object>>> sortExpressions 
            = new Dictionary<string, Expression<Func<ProcessEntity, object>>>
        {
            { nameof(ProcessEntity.process_code).Split("_")[1], x => x.process_code },
            { nameof(ProcessEntity.process_name).Split("_")[1], x => x.process_name },
            { nameof(ProcessEntity.process_description).Split("_")[1], x => x.process_description },
            { nameof(ProcessEntity.created_at).Split("_")[0], x => x.created_at }
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
        private Expression<Func<ProcessEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<ProcessEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<ProcessEntity, bool>> AddSearchCriteria(Expression<Func<ProcessEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.process_name.ToUpper().Contains(search.ToUpper()) ||
                x.process_description.ToLower().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<ProcessEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<ProcessEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<ProcessEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.process_code == code;
        }

        public static Expression<Func<ProcessEntity, bool>> GetByTypeExpression(Guid typeId)
        {
            return x => true && x.process_type_id == typeId;
        }


    }
}
