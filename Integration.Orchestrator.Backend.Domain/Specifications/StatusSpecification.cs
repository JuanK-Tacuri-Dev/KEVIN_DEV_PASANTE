﻿using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class StatusSpecification : ISpecification<StatusEntity>
    {
        public Expression<Func<StatusEntity, bool>> Criteria { get; private set; }

        public Expression<Func<StatusEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<StatusEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public StatusSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<StatusEntity, object>>> sortExpressions 
            = new Dictionary<string, Expression<Func<StatusEntity, object>>>
        {
            { nameof(StatusEntity.status_key), x => x.status_key },
            { nameof(StatusEntity.status_text), x => x.status_text },
            { nameof(StatusEntity.status_color), x => x.status_color },
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
        private Expression<Func<StatusEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<StatusEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<StatusEntity, bool>> AddSearchCriteria(Expression<Func<StatusEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.status_key.ToUpper().Contains(search.ToUpper()) ||
                x.status_text.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<StatusEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<StatusEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<StatusEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.status_key == code;
        }
    }
}
