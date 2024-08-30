using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class PropertySpecification : ISpecification<PropertyEntity>
    {
        public Expression<Func<PropertyEntity, bool>> Criteria { get; private set; }

        public Expression<Func<PropertyEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<PropertyEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public PropertySpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<PropertyEntity, object>>> sortExpressions 
            = new Dictionary<string, Expression<Func<PropertyEntity, object>>>
        {
            { nameof(PropertyEntity.type_id), x => x.type_id },
            { nameof(PropertyEntity.property_code), x => x.property_code }
        };
        private void SetupPagination(PaginatedModel model)
        {
            Skip = (model.Page - 1) * model.Rows;
            Limit = model.Rows;
        }

        private void SetupOrdering(PaginatedModel model)
        {
            if (sortExpressions.TryGetValue(model.SortBy, out var expression))
            {
                if (model.SortOrder == SortOrdering.Ascending)
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

        private Expression<Func<PropertyEntity, bool>> AddSearchCriteria(Expression<Func<PropertyEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.property_code.ToUpper().Contains(search.ToUpper()));
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
        
        public static Expression<Func<PropertyEntity, bool>> GetByNameAndEntityIdExpression(string name, Guid entityId)
        {
            return x => true && x.property_name == name && x.entity_id == entityId;
        }        


    }
}
