using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces
{
    public interface IConnectionService<T>
    {
        Task InsertAsync(T connection);
        Task UpdateAsync(T connection);
        Task DeleteAsync(T connection);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByServerIdAsync(Guid serverid);
        Task<T> GetByAdapterIdAsync(Guid adapterid);
        Task<T> GetByRepositoryIdAsync(Guid repositoryId);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
