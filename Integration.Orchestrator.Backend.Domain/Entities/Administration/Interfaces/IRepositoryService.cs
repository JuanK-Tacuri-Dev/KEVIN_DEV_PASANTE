using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces
{
    public interface IRepositoryService<T>
    {
        Task InsertAsync(T repository);
        Task UpdateAsync(T repository);
        Task DeleteAsync(T repository);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
