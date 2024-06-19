using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces
{
    public interface IIntegrationService<T>
    {
        Task InsertAsync(T connection);
        Task UpdateAsync(T connection);
        Task DeleteAsync(T connection);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
