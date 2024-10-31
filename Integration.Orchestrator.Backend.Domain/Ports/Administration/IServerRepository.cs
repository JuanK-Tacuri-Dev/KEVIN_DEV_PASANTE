using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Dto.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Server;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Ports.Configurador
{
    public interface IServerRepository<T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> specification);
        Task<T> GetByCodeAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetByTypeAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<ServerDto>> GetAllAsync(ISpecification<T> specification);
        Task<IEnumerable<ServerResponseTest>> GetAllAsyncTest(ISpecification<T> specification);
        public Task<long> GetTotalRows(ISpecification<T> specification);
        Task<bool> ValidateNameURL(ServerEntity entity);
    }
}
