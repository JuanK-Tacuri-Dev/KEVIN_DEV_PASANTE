using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurador;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces
{
    public interface IIntegrationService<T>
    {
        Task InsertAsync(T integration);
        Task UpdateAsync(T integration);
        Task DeleteAsync(T integration);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByProcessIdAsync(Guid idProcess, Guid idStatusActive);
        Task<IEnumerable<IntegrationResponseModel>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
