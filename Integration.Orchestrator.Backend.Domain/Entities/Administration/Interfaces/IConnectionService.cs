using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces
{
    public interface IConnectionService<T>
    {
        Task InsertAsync(T connection);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<T>> GetByTypeAsync(string type);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
