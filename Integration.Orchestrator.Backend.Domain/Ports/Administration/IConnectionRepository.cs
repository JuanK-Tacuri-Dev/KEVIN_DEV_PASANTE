﻿using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Ports.Administration
{
    public interface IConnectionRepository<T>
    {
        Task InsertAsync(T entity);
        Task<T> GetByCodeAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetByTypeAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification);
        public Task<long> GetTotalRows(ISpecification<T> specification);
    }
}
