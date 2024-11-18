using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class RepositorySpecification : ISpecification<RepositoryEntity>
    {
        public Expression<Func<RepositoryEntity, bool>> Criteria { get; set; }

        public List<LookupSpecification<RepositoryEntity>> Includes { get; } = [];
        public Expression<Func<RepositoryEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<RepositoryEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public RepositorySpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<RepositoryEntity, object>>> sortExpressions 
            = new Dictionary<string, Expression<Func<RepositoryEntity, object>>>
        {
            { nameof(RepositoryEntity.repository_code).Split("_")[1], x => x.repository_code },
            { nameof(RepositoryEntity.repository_port).Split("_")[1], x => x.repository_port },
            { nameof(RepositoryEntity.repository_userName).Split("_")[1], x => x.repository_userName },
            { nameof(RepositoryEntity.repository_password).Split("_")[1], x => x.repository_password },
            { nameof(RepositoryEntity.repository_databaseName).Split("_")[1], x => x.repository_databaseName },
            { "authTypeId", x => x.auth_type_id },
            { "statusId", x => x.status_id },
            { nameof(RepositoryEntity.updated_at).Split("_")[0], x => x.updated_at },
            { nameof(RepositoryEntity.created_at).Split("_")[0], x => x.created_at }
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
        private Expression<Func<RepositoryEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<RepositoryEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<RepositoryEntity, bool>> AddSearchCriteria(Expression<Func<RepositoryEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.repository_databaseName.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<RepositoryEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<RepositoryEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<RepositoryEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.repository_code == code;
        }
    }
}
