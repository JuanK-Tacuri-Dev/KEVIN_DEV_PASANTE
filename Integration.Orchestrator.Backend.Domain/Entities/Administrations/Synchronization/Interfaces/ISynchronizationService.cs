using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization.Interfaces
{
    public interface ISynchronizationService<T>
    {
        Task InsertAsync(T synchronization);
        Task UpdateAsync(T synchronization);
        Task DeleteAsync(T synchronization);
        Task<T> GetByIdAsync(Guid id);
        Task<string> GetStatusByIdAsync(Guid idStatus);
        Task<IEnumerable<T>> GetByFranchiseIdAsync(Guid franchiseId);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);



    }
}
