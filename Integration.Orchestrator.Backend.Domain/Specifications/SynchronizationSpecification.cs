using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class SynchronizationSpecification : ISpecification<SynchronizationEntity>
    {
        public Expression<Func<SynchronizationEntity, bool>> Criteria { get; private set; }

        public List<LookupSpecification<SynchronizationEntity>> Includes { get; } = [];
        public Expression<Func<SynchronizationEntity, object>> OrderBy { get; private set; }

        public Expression<Func<SynchronizationEntity, object>> OrderByDescending { get; private set; }
        public Dictionary<string, object> AdditionalFilters { get; } = [];
        public int Skip { get; private set; }

        public int Limit { get; private set; }



        public SynchronizationSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            AddFilterSearch(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
            SetupIncludes();
        }

        private static readonly Dictionary<string, Expression<Func<SynchronizationEntity, object>>> sortExpressions = new()
            {
            { nameof(SynchronizationEntity.synchronization_name).Split("_")[1], x => x.synchronization_name },
            { nameof(SynchronizationEntity.synchronization_observations).Split("_")[1], x => x.synchronization_observations },
            { nameof(SynchronizationEntity.synchronization_code).Split("_")[1], x => x.synchronization_code },
            { nameof(SynchronizationEntity.synchronization_hour_to_execute).Split("_")[1], x => x.synchronization_hour_to_execute },
            { nameof(SynchronizationEntity.updated_at).Split("_")[0], x => x.updated_at },
            { nameof(SynchronizationEntity.created_at).Split("_")[0], x => x.created_at },
             { "status_id", x => x.status_id },
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
        private Expression<Func<SynchronizationEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<SynchronizationEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }


        private Expression<Func<SynchronizationEntity, bool>> AddSearchCriteria(Expression<Func<SynchronizationEntity, bool>> criteria,string search,IEnumerable<SynchronizationStatusEntity> statusEntities)
        {
            if (!string.IsNullOrEmpty(search))
            {
                // Obtener los IDs de los estados que coinciden con la búsqueda
                var matchingStatusIds = statusEntities
                    .Where(status => status.synchronization_status_key.ToUpper().Contains(search.ToUpper()))
                    .Select(status => status.id) // Id proviene de la herencia de Entity<Guid>
                    .ToList();

                // Agregar criterios basados en `synchronization_name` o `status_id` relacionado
                criteria = criteria.And(x =>
                    x.synchronization_name.ToUpper().Contains(search.ToUpper()) ||
                    matchingStatusIds.Contains(x.status_id));
            }

            return criteria;
        }

        private Expression<Func<SynchronizationEntity, bool>> AddSearchCriteria(Expression<Func<SynchronizationEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.synchronization_name.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }
        private void AddFilterSearch(PaginatedModel paginatedModel)
        {
            if (paginatedModel.filter_Option!=null && paginatedModel.filter_Option.Any())
            {
                foreach (var item in paginatedModel.filter_Option)
                {
                    if (sortExpressions.TryGetValue(item.filter_column, out var filter))
                    {
                        AdditionalFilters.Add(item.filter_column,item.filter_search);
                    }
                }

            }
        }

        private void SetupIncludes()
        {
            Includes.Add(new LookupSpecification<SynchronizationEntity> { Collection = "Integration_SynchronizationStates", LocalField = "status_id", ForeignField = "_id", As = "SynchronizationStates" });
        }
        public static Expression<Func<SynchronizationEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<SynchronizationEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<SynchronizationEntity, bool>> GetByCodeExpression(string code)
        {
            return x => x.synchronization_code == code;
        }

        public static Expression<Func<SynchronizationEntity, bool>> GetByFranchiseIdExpression(Guid franchiseId)
        {
            return x => x.franchise_id == franchiseId;
        }
        public static Expression<Func<SynchronizationEntity, bool>> GetByIntegrationIdExpression(Guid idIntegration, Guid IdStatusCanceled)
        {
            return x => x.integrations.Contains(idIntegration) && x.status_id != IdStatusCanceled;
        }
    }
}
