using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Catalog;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces
{
    public interface ICatalogService<T>
    {
        Task InsertAsync(T catalog);
        Task UpdateAsync(T catalog);
        Task DeleteAsync(T catalog);
        Task<T> GetByIdAsync(Guid id);
        Task<CatalogEntity> GetByCodeAsync(int code);
        Task<IEnumerable<CatalogEntity>> GetByFatherAsync(int fatherCode);
        Task<IEnumerable<CatalogResponseModel>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
