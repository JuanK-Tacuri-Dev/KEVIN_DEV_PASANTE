using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces
{
    public interface IIntegrationService<T>
    {
        Task InsertAsync(T integration);
        Task UpdateAsync(T integration);
        Task DeleteAsync(T integration);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
