using Integration.Orchestrator.Backend.Domain.Models;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces
{
    public interface IStatusService<T>
    {
        Task InsertAsync(T connection);
        Task<IEnumerable<T>> GetAllPaginatedAsync(PaginatedModel paginatedModel);
        Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel);
    }
}
