using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurator.Entity;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator.Interfaces
{
    public interface IEntitiesService<T>
    {
        Task InsertAsync(T entities);
        Task UpdateAsync(T entities);
        Task DeleteAsync(T entities);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<T>> GetByTypeIdAsync(Guid typeId);
        Task<IEnumerable<T>> GetByRepositoryIdAsync(Guid repositoryId, Guid idStatusActive);
        Task<IEnumerable<EntityResponseModel>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);

    }
}
