using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Ports.Administration
{
    public interface IEntitiesRepository<T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> specification);
        Task<T> GetByCodeAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetByTypeIdAsync(Expression<Func<T, bool>> specification); 
        Task<IEnumerable<T>> GetByRepositoryIdAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification);
        public Task<long> GetTotalRows(ISpecification<T> specification);
        Task<IEnumerable<T>> GetByNameAndRepositoryIdAsync(Expression<Func<T, bool>> specification);
    }
}
