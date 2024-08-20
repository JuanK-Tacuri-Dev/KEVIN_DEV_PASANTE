using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces
{
    public interface ICatalogService<T>
    {
        Task InsertAsync(T catalog);
        Task UpdateAsync(T catalog);
        Task DeleteAsync(T catalog);
        Task<T> GetByIdAsync(Guid id);
        Task<CatalogEntity> GetByCodeAsync(string code);
        Task<IEnumerable<CatalogEntity>> GetByFatherAsync(Guid fatherId);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
