using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class AdapterSpecification : ISpecification<AdapterEntity>
    {
        public Expression<Func<AdapterEntity, bool>> Criteria { get; private set; }

        public Expression<Func<AdapterEntity, object>> OrderBy { get; private set; }

        public Expression<Func<AdapterEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }

        public AdapterSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<AdapterEntity, object>>> sortExpressions
            = new Dictionary<string, Expression<Func<AdapterEntity, object>>>
        {
            { nameof(AdapterEntity.adapter_name).Split("_")[1], x => x.adapter_name },
            { nameof(AdapterEntity.adapter_code).Split("_")[1], x => x.adapter_code },
            { nameof(AdapterEntity.created_at), x => x.created_at }
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
        private Expression<Func<AdapterEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<AdapterEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<AdapterEntity, bool>> AddSearchCriteria(Expression<Func<AdapterEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.adapter_name.ToLower().Contains(search.ToLower()));
            }

            return criteria;
        }

        public static Expression<Func<AdapterEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<AdapterEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<AdapterEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.adapter_code == code;
        }

        public static Expression<Func<AdapterEntity, bool>> GetByTypeExpression(Guid typeId)
        {
            return x => true && x.type_id == typeId;
        }


    }
}
