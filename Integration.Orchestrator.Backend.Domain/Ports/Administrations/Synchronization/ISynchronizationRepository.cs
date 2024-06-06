using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Ports.Administrations.Synchronization
{
    public interface ISynchronizationRepository<T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetByFranchiseIdAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification);
        public Task<long> GetTotalRows(ISpecification<T> specification);


    }
}
