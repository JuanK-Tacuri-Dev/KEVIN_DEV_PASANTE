using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces
{
    public interface ISynchronizationStatesService<T>
    {
        Task InsertAsync(T synchronizationStates);
        Task UpdateAsync(T synchronizationStates);
        Task DeleteAsync(T synchronizationStates);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByKeyAsync(string key);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
