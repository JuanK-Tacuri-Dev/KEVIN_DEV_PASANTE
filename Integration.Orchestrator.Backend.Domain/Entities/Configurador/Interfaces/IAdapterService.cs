using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces
{
    public interface IAdapterService<T>
    {
        Task InsertAsync(T adapter);
        Task UpdateAsync(T adapter);
        Task DeleteAsync(T adapter);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<T>> GetByTypeAsync(Guid typeId);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
