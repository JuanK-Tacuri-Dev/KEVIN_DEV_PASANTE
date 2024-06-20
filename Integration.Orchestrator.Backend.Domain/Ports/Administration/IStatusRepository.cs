using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Ports.Administration
{
    public interface IStatusRepository<T>
    {
        Task InsertAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync(ISpecification<T> specification);
        public Task<long> GetTotalRows(ISpecification<T> specification);
    }
}
