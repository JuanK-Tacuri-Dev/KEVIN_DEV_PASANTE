using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Ports.Administrations.Synchronization
{
    public interface ISynchronizationRepository<T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllPaginatedAsync(ISpecification<T> specification);
        Task<long> GetTotalRows(ISpecification<T> specification);
    }
}
