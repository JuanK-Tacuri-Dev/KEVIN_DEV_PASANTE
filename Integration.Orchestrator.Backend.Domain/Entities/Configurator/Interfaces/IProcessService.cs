using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurator;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator.Interfaces
{
    public interface IProcessService<T>
    {
        Task InsertAsync(T process);
        Task UpdateAsync(T process);
        Task DeleteAsync(T process);
        Task<T> GetByIdAsync(Guid id);
        Task<IEnumerable<T>> GetByEntityActiveIdAsync(Guid idEntity, Guid idStatusActive);
        Task<IEnumerable<T>> GetByPropertyActiveIdAsync(Guid idProperty, Guid idStatusActive);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<T>> GetByTypeAsync(Guid typeId);
        Task<T> GetByConnectionIdAsync(Guid connectionid, Guid idStatusActive);
        Task<IEnumerable<ProcessResponseModel>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
