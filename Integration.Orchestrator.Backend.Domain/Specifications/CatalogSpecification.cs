using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Models;
using MongoDB.Bson.Serialization.Serializers;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class CatalogSpecification : ISpecification<CatalogEntity>
    {
        public Expression<Func<CatalogEntity, bool>> Criteria { get; private set; }
        public List<LookupSpecification<CatalogEntity>> Includes { get; } = [];
        public Expression<Func<CatalogEntity, object>> OrderBy { get; private set; }

        public Expression<Func<CatalogEntity, object>> OrderByDescending { get; private set; }
        public Dictionary<string, object> AdditionalFilters { get; } = [];

        public int Skip { get; private set; }

        public int Limit { get; private set; }

        public CatalogSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            AddFilterSearch(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
            SetupIncludes();
        }

        private static readonly Dictionary<string, Expression<Func<CatalogEntity, object>>> sortExpressions = new()
        {
            { Utilities.GetSafeKey(nameof(CatalogEntity.catalog_code), 1), x => x.catalog_code },
            { Utilities.GetSafeKey(nameof(CatalogEntity.catalog_name), 1), x => x.catalog_name },
            { Utilities.GetSafeKey(nameof(CatalogEntity.catalog_detail), 1), x => x.catalog_detail },
            { Utilities.GetSafeKey(nameof(CatalogEntity.catalog_value), 1), x => x.catalog_value },
            { Utilities.GetSafeKey(nameof(CatalogEntity.updated_at), 0), x => x.updated_at },
            { Utilities.GetSafeKey(nameof(CatalogEntity.created_at), 0), x => x.created_at },
            { "fatherCode", x => x.father_code },
            { "isfather", x => x.is_father },
            { "statusId", x => x.status_id }
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
        private void AddFilterSearch(PaginatedModel paginatedModel)
        {
            if (paginatedModel.filter_Option != null && paginatedModel.filter_Option.Any())
            {
                foreach (var item in paginatedModel.filter_Option)
                {
                    if (sortExpressions.TryGetValue(item.filter_column, out var filter))
                    {
                        AdditionalFilters.Add(item.filter_column, item.filter_search);
                    }
                }

            }
        }
        private Expression<Func<CatalogEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<CatalogEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel);

            return criteria;
        }

        private Expression<Func<CatalogEntity, bool>> AddSearchCriteria(Expression<Func<CatalogEntity, bool>> criteria, PaginatedModel model)
        {
            if (!string.IsNullOrEmpty(model.Search))
            {
                var searchTerm = model.Search.ToUpper();

                criteria = criteria.And(x =>
                    x.catalog_name.ToUpper().Contains(searchTerm) ||
                    (model.Sort_field == "isfather" && x.is_father.ToString().ToUpper() == searchTerm) ||
                    x.catalog_detail.ToUpper().Contains(searchTerm) ||
                    x.catalog_value.ToUpper().Contains(searchTerm));
            }

            return criteria;
        }
        private void SetupIncludes()
        {
            Includes.Add(new LookupSpecification<CatalogEntity> { Collection = "Integration_Status", LocalField = "status_id", ForeignField = "_id", As = "Status" });
        }
        public static Expression<Func<CatalogEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<CatalogEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<CatalogEntity, bool>> GetByExpression(Expression<Func<CatalogEntity, bool>> expresion) => expresion;
        public static Expression<Func<CatalogEntity, bool>> GetByFatherExpression(int fatherCode)
        {
            return x => true && x.father_code == fatherCode;
        }

        public static Expression<Func<CatalogEntity, bool>> GetByCodeExpression(int code)
        {
            return x => true && x.catalog_code == code;
        }
    }
}
