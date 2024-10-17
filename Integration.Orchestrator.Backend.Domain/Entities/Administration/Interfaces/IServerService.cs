using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces
{
    public interface IServerService<T>
    {
        Task InsertAsync(T server);
        Task UpdateAsync(T server);
        Task DeleteAsync(T server);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<T>> GetByTypeAsync(Guid typeId);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
