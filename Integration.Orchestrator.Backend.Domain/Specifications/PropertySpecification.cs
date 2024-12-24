using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class PropertySpecification : ISpecification<PropertyEntity>
    {
        public Expression<Func<PropertyEntity, bool>> Criteria { get; set; }

        public List<LookupSpecification<PropertyEntity>> Includes { get; } = [];
        public Expression<Func<PropertyEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<PropertyEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public PropertySpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
            SetupIncludes();
        }

        private static readonly Dictionary<string, Expression<Func<PropertyEntity, object>>> sortExpressions = new()
            {
            { nameof(PropertyEntity.property_name).Split("_")[1], x => x.property_name },
            { nameof(PropertyEntity.property_code).Split("_")[1], x => x.property_code },
            { "typeId", x => x.type_id },
            { "entityId", x => x.entity_id },
            { "status", x => x.status_id },
            { nameof(PropertyEntity.updated_at).Split("_")[0], x => x.updated_at },
            { nameof(PropertyEntity.created_at).Split("_")[0], x => x.created_at }
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
        private Expression<Func<PropertyEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<PropertyEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private void SetupIncludes()
        {
            Includes.Add(new LookupSpecification<PropertyEntity> { Collection = "Integration_Catalog", LocalField = "type_id", ForeignField = "_id", As = "CatalogData" });
            Includes.Add(new LookupSpecification<PropertyEntity> { Collection = "Integration_Entity", LocalField = "entity_id", ForeignField = "_id", As = "EntityData" });
        }

        private Expression<Func<PropertyEntity, bool>> AddSearchCriteria(Expression<Func<PropertyEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria
                    .And(x =>x.property_code.ToUpper().Contains(search.ToUpper()) || 
                             x.property_name.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<PropertyEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<PropertyEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<PropertyEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.property_code == code;
        }

        public static Expression<Func<PropertyEntity, bool>> GetByTypeExpression(Guid typeId)
        {
            return x => true && x.type_id == typeId;
        }
        
        public static Expression<Func<PropertyEntity, bool>> GetByEntityActiveExpression(Guid entityId, Guid idStatusActive)
        {
            return x => true && x.entity_id == entityId && x.status_id==idStatusActive;
        }
        public static Expression<Func<PropertyEntity, bool>> GetByEntityExpression(Guid entityId, Guid idStatusActive)
        {
            return x => true && x.entity_id == entityId && x.status_id == idStatusActive;
        }
        public static Expression<Func<PropertyEntity, bool>> GetByNameAndEntityIdExpression(string name, Guid entityId)
        {
            return x => true && x.property_name.ToUpper() == name.ToUpper() && x.entity_id == entityId;
        }        


    }
}
