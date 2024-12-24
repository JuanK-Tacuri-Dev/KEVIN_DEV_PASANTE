using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurator;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator.Interfaces
{
    public interface IRepositoryService<T>
    {
        Task InsertAsync(T repository);
        Task UpdateAsync(T repository);
        Task DeleteAsync(T repository);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<RepositoryResponseModel>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
