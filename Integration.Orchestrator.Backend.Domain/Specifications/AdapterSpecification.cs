using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class AdapterSpecification : ISpecification<AdapterEntity>
    {
        public Expression<Func<AdapterEntity, bool>> Criteria { get;  set; }

        public List<LookupSpecification<AdapterEntity>> Includes { get; } = [];
        public Expression<Func<AdapterEntity, object>> OrderBy { get; private set; }

        public Expression<Func<AdapterEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public AdapterSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
            SetupIncludes();
        }
        private static readonly Dictionary<string, Expression<Func<AdapterEntity, object>>> sortExpressions = new()
        {
            { Utilities.GetSafeKey(nameof(AdapterEntity.adapter_name), 1), x => x.adapter_name },
            { Utilities.GetSafeKey(nameof(AdapterEntity.adapter_code), 1), x => x.adapter_code },
            { Utilities.GetSafeKey(nameof(AdapterEntity.adapter_version), 1), x => x.adapter_version },
            { "typeAdapterName", x => x.type_id },
            { "statusId", x => x.status_id },
            { Utilities.GetSafeKey(nameof(AdapterEntity.updated_at), 0), x => x.updated_at },
            { Utilities.GetSafeKey(nameof(AdapterEntity.created_at), 0), x => x.created_at }
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

        private void SetupIncludes()
        {
            Includes.Add(new LookupSpecification<AdapterEntity> { Collection = "Integration_Catalog", LocalField = "type_id", ForeignField = "_id", As = "CatalogData" });
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
