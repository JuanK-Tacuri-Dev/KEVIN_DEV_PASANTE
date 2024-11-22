using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurador;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces
{
    public interface IConnectionService<T>
    {
        Task InsertAsync(T connection);
        Task UpdateAsync(T connection);
        Task DeleteAsync(T connection);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByServerIdAsync(Guid serverid, Guid idStatusActive);
        Task<T> GetByAdapterIdAsync(Guid adapterid, Guid idStatusActive);
        Task<T> GetByRepositoryIdAsync(Guid repositoryId, Guid idStatusActive);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<ConnectionResponseModel>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
