using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Models;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class SynchronizationSpecification : ISpecification<SynchronizationEntity, PaginatedModel>
    {
        public Expression<Func<SynchronizationEntity, bool>> Criteria { get; private set; }

        public Expression<Func<SynchronizationEntity, object>> OrderBy { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }

        public SynchronizationSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
        }
        public Expression<Func<SynchronizationEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<SynchronizationEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<SynchronizationEntity, bool>> AddSearchCriteria(Expression<Func<SynchronizationEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.status.ToUpper().Contains(search.ToUpper()) ||
                x.observations.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        
    }
}
