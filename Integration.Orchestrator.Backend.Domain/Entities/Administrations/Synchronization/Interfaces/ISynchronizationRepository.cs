using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization.Interfaces
{
    public interface ISynchronizationRepository<T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetByFranchiseIdAsync(Guid franchiseId);
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification);
        public Task<long> GetTotalRows(ISpecification<T> specification);


    }
}
