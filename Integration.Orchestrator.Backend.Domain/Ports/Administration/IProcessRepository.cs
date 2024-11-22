﻿using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Models.Configurador;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Ports.Configurador
{
    public interface IProcessRepository<T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetByEntitiesIdAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetByPropertiesIdAsync(Expression<Func<T, bool>> specification);
        Task<T> GetByCodeAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetByTypeAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<ProcessResponseModel>> GetAllAsync(ISpecification<T> specification);
        public Task<long> GetTotalRows(ISpecification<T> specification);
    }
}
