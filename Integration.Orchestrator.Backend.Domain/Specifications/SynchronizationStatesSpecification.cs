﻿using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Models;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Specifications
{
    public class SynchronizationStatesSpecification : ISpecification<SynchronizationStatusEntity>
    {
        public Expression<Func<SynchronizationStatusEntity, bool>> Criteria { get; private set; }

        public Expression<Func<SynchronizationStatusEntity, object>> OrderBy { get; private set; }
        
        public Expression<Func<SynchronizationStatusEntity, object>> OrderByDescending { get; private set; }

        public int Skip { get; private set; }

        public int Limit { get; private set; }
       
        public SynchronizationStatesSpecification(PaginatedModel paginatedModel)
        {
            Criteria = BuildCriteria(paginatedModel);
            SetupPagination(paginatedModel);
            SetupOrdering(paginatedModel);
        }

        private static readonly Dictionary<string, Expression<Func<SynchronizationStatusEntity, object>>> sortExpressions 
            = new Dictionary<string, Expression<Func<SynchronizationStatusEntity, object>>>
        {
            { nameof(SynchronizationStatusEntity.synchronization_status_key), x => x.synchronization_status_key },
            { nameof(SynchronizationStatusEntity.synchronization_status_text), x => x.synchronization_status_text },
            { nameof(SynchronizationStatusEntity.synchronization_status_color), x => x.synchronization_status_color },
            { nameof(SynchronizationStatusEntity.synchronization_status_background), x => x.synchronization_status_background },
            { nameof(SynchronizationStatusEntity.updated_at), x => x.updated_at },
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
        private Expression<Func<SynchronizationStatusEntity, bool>> BuildCriteria(PaginatedModel paginatedModel)
        {
            var criteria = (Expression<Func<SynchronizationStatusEntity, bool>>)(x => true);

            // Apply base criteria

            // Apply search criteria
            criteria = AddSearchCriteria(criteria, paginatedModel.Search);

            return criteria;
        }

        private Expression<Func<SynchronizationStatusEntity, bool>> AddSearchCriteria(Expression<Func<SynchronizationStatusEntity, bool>> criteria, string search)
        {
            if (!string.IsNullOrEmpty(search))
            {
                criteria = criteria.And(x =>
                x.synchronization_status_key.ToUpper().Contains(search.ToUpper()) ||
                x.synchronization_status_text.ToUpper().Contains(search.ToUpper()));
            }

            return criteria;
        }

        public static Expression<Func<SynchronizationStatusEntity, bool>> GetByIdExpression(Guid id)
        {
            return BaseSpecification<SynchronizationStatusEntity>.GetByUuid(x => x.id, id);
        }

        public static Expression<Func<SynchronizationStatusEntity, bool>> GetByCodeExpression(string code)
        {
            return x => true && x.synchronization_status_key == code;
        }

    }
}
