using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces
{
    public interface IStatusService<T>
    {
        Task InsertAsync(T status);
        Task UpdateAsync(T status);
        Task DeleteAsync(T status);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByKeyAsync(string key);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
