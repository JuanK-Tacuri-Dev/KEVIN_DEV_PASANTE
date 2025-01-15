using Integration.Orchestrator.Backend.Domain.Models.Configurator;
using Integration.Orchestrator.Backend.Domain.Models.Configurator.Transformation;
using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Ports.Configurator
{
    public interface ITransformationRepository<T>
    {
        Task<IEnumerable<TransformationResponseModel>> GetAllAsync(ISpecification<T> specification);
        public Task<long> GetTotalRowsAsync(ISpecification<T> specification);
    }
}
