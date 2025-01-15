using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class TransformationSpecification : ISpecification<TransformationEntity>
    {
        public Expression<Func<TransformationEntity, bool>> Criteria { get; private set; }

        public List<LookupSpecification<TransformationEntity>> Includes { get; } = [];
        public Expression<Func<TransformationEntity, object>> OrderBy { get; private set; }

        public Expression<Func<TransformationEntity, object>> OrderByDescending { get; private set; }
        public Dictionary<string, object> AdditionalFilters { get; } = [];
        public int Skip { get; private set; }

        public int Limit { get; private set; }



        public TransformationSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            AddFilterSearch(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
            SetupIncludes();
        }

        private static readonly Dictionary<string, Expression<Func<TransformationEntity, object>>> sortExpressions = new()
            {

            { Utilities.GetSafeKey(nameof(TransformationEntity.transformation_name), 1), x => x.transformation_name },
            { Utilities.GetSafeKey(nameof(TransformationEntity.transformation_code), 1), x => x.transformation_code },
            { nameof(TransformationEntity.description), x => x.description }
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
        private Expression<Func<TransformationEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<TransformationEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }



        private Expression<Func<TransformationEntity, bool>> AddSearchCriteria(Expression<Func<TransformationEntity, bool>> criteria, string search)
        {

            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.transformation_name.ToUpper().Contains(search.ToUpper()) ||
                x.transformation_code.ToUpper().Contains(search.ToUpper()) ||
                x.description.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
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

        private void SetupIncludes()
        {
            //Includes.Add(new LookupSpecification<TransformationEntity> { Collection = "Integration_SynchronizationStates", LocalField =// "status_id", ForeignField = "_id", As = "SynchronizationStates" });
        }
        public static Expression<Func<TransformationEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<TransformationEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<TransformationEntity, bool>> GetByCodeExpression(string code)
        {
            return x => x.transformation_code == code;
        }

        public static Expression<Func<TransformationEntity, bool>> GetByNameExpression(string name)
        {
            return x => x.transformation_name == name;
        }
        public static Expression<Func<TransformationEntity, bool>> GetByExpression(Expression<Func<TransformationEntity, bool>> expresion) => expresion;

    }
}
