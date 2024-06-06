using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization.Interfaces
{
    public interface ISynchronizationStatesService<T>
    {
        Task InsertAsync(T synchronization);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
