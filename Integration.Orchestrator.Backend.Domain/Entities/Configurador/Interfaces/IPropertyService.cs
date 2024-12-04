using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Property;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces
{
    public interface IPropertyService<T>
    {
        Task InsertAsync(T property);
        Task UpdateAsync(T property);
        Task DeleteAsync(T property);
        Task<T> GetByIdAsync(Guid id);
        Task<T> GetByCodeAsync(string code);
        Task<IEnumerable<T>> GetByTypeIdAsync(Guid typeId);
        Task<IEnumerable<T>> GetByEntitysIdAsync(Guid entityId, Guid idStatusActive);
        Task<T> GetByEntityIdAsync(Guid entityId,Guid idStatusActive);
        Task<IEnumerable<PropertyResponseModel>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
