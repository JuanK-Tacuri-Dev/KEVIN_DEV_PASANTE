using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces
{
    public interface ICatalogService<T>
    {
        Task InsertAsync(T catalog);
        Task UpdateAsync(T catalog);
        Task DeleteAsync(T catalog);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetByTypeAsync(string type);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
