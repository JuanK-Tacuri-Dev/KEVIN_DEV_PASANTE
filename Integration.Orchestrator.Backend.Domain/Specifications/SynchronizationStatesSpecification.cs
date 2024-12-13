using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class SynchronizationStatesSpecification : ISpecification<SynchronizationStatusEntity>
    {
        public Expression<Func<SynchronizationStatusEntity, bool>> Criteria { get; private set; }

        public List<LookupSpecification<SynchronizationStatusEntity>> Includes { get; } = [];
        public Expression<Func<SynchronizationStatusEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<SynchronizationStatusEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public SynchronizationStatesSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
            SetupIncludes();
        }

        private static readonly Dictionary<string, Expression<Func<SynchronizationStatusEntity, object>>> sortExpressions = new()
        {
            { nameof(SynchronizationStatusEntity.synchronization_status_key).Split("_")[2], x => x.synchronization_status_key },
            { nameof(SynchronizationStatusEntity.synchronization_status_text).Split("_")[2], x => x.synchronization_status_text },
            { nameof(SynchronizationStatusEntity.synchronization_status_color).Split("_")[2], x => x.synchronization_status_color },
            { nameof(SynchronizationStatusEntity.synchronization_status_background).Split("_")[2], x => x.synchronization_status_background },
            { nameof(SynchronizationStatusEntity.created_at).Split("_")[0], x => x.created_at },
            { nameof(SynchronizationStatusEntity.updated_at).Split("_")[0], x => x.updated_at },
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
        private Expression<Func<SynchronizationStatusEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<SynchronizationStatusEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<SynchronizationStatusEntity, bool>> AddSearchCriteria(Expression<Func<SynchronizationStatusEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.synchronization_status_text.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }
        private void SetupIncludes()
        {
            
        }
        public static Expression<Func<SynchronizationStatusEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<SynchronizationStatusEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<SynchronizationStatusEntity, bool>> GetByKeyExpression(string key)
        {
            return x => true && x.synchronization_status_key == key;
        }
        public static Expression<Func<SynchronizationStatusEntity, bool>> GetByStatusIdExpression(string [] CodeStatus)
        {
            return x => true && CodeStatus.Contains(x.synchronization_status_key);
        }


    }
}
