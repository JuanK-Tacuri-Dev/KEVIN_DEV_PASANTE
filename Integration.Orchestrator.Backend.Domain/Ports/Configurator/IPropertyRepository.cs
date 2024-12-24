using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Models.Configurator.Property;
using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Ports.Configurator
{
    public interface IPropertyRepository<T>
    {
        Task InsertAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<T> GetByIdAsync(Expression<Func<T, bool>> specification);
        Task<T> GetByCodeAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetByTypeAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<T>> GetByEntitysAsync(Expression<Func<T, bool>> specification);
        Task<T> GetByEntityAsync(Expression<Func<T, bool>> specification);
        Task<IEnumerable<PropertyResponseModel>> GetAllAsync(ISpecification<T> specification);
        public Task<long> GetTotalRows(ISpecification<T> specification);
        Task<bool> ValidateNameAndEntity(PropertyEntity property);
    }
}
