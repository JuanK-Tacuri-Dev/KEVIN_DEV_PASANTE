using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces
{
    public interface ISynchronizationService<T>
    {
        Task InsertAsync(T synchronization);
        Task UpdateAsync(T synchronization);
        Task DeleteAsync(T synchronization);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<T>> GetByFranchiseIdAsync(Guid franchiseId);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);



    }
}
