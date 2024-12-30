using System.Linq.Expressions;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Models.Configurator;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Ports.Configurator
{
    public interface IAdapterRepository<T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> specification);
        Task<T> GetByCodeAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetByTypeAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<AdapterResponseModel>> GetAllAsync(ISpecification<T> specification);
        public Task<long> GetTotalRows(ISpecification<T> specification);
        public Task<bool> ValidateAdapterNameVersion(AdapterEntity entity);
    }
}
