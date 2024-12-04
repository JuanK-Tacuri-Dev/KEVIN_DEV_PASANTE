using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class EntitiesSpecification : ISpecification<EntitiesEntity>
    {
        public Expression<Func<EntitiesEntity, bool>> Criteria { get; set; }
        public List<LookupSpecification<EntitiesEntity>> Includes { get; } = [];

        public Expression<Func<EntitiesEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<EntitiesEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public EntitiesSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
            SetupIncludes();
        }

        private static readonly Dictionary<string, Expression<Func<EntitiesEntity, object>>> sortExpressions = new()
            {
            { nameof(EntitiesEntity.entity_code).Split("_")[1], x => x.entity_code },
            { nameof(EntitiesEntity.entity_name).Split("_")[1], x => x.entity_name },
            { "typeId", x => x.type_id },
            { "repositoryId", x => x.repository_id },
            { "statusId", x => x.status_id },
            { nameof(EntitiesEntity.updated_at).Split("_")[0], x => x.updated_at },
            { nameof(EntitiesEntity.created_at).Split("_")[0], x => x.created_at }
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
        private Expression<Func<EntitiesEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<EntitiesEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private void SetupIncludes()
        {
            Includes.Add(new LookupSpecification<EntitiesEntity> { Collection = "Integration_Catalog", LocalField = "type_id", ForeignField = "_id", As = "CatalogData" });
            Includes.Add(new LookupSpecification<EntitiesEntity> { Collection = "Integration_Repository", LocalField = "repository_id", ForeignField = "_id", As = "RepositoryData" });

        }
        private Expression<Func<EntitiesEntity, bool>> AddSearchCriteria(Expression<Func<EntitiesEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria
                    .And(x => x.entity_name.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<EntitiesEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<EntitiesEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<EntitiesEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.entity_code == code;
        }

        public static Expression<Func<EntitiesEntity, bool>> GetByTypeExpression(Guid typeId)
        {
            return x => true && x.type_id == typeId;
        }

        public static Expression<Func<EntitiesEntity, bool>> GetByRepositoryIdExpression(Guid repositoryId, Guid idStatusActive)
        {
            return x => true && x.repository_id == repositoryId && x.status_id== idStatusActive;
        }
                
        public static Expression<Func<EntitiesEntity, bool>> GetByNameAndRepositoryIdExpression(string name, Guid repositoryId)
        {
            return x => true && x.entity_name.ToUpper() == name.ToUpper() && x.repository_id == repositoryId;
        }

        private Expression<Func<EntitiesEntity, bool>> ValidateEntityProperties(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<EntitiesEntity, bool>>)(x => true);

            return criteria;
        }

        public static Expression<Func<EntitiesEntity, bool>> ValidateEntityProperties(EntitiesEntity input)
        {
            return x =>
                        x.entity_name.ToUpper() == input.entity_name.ToUpper() &&
                        x.entity_code.ToUpper() == input.entity_code.ToUpper() &&
                        x.type_id == input.type_id &&
                        x.repository_id == input.repository_id &&
                        x.status_id == input.status_id;
        }
    }
}