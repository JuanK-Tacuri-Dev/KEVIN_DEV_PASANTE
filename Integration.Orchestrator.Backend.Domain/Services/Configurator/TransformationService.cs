using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurator;
using Integration.Orchestrator.Backend.Domain.Models.Configurator.Transformation;
using Integration.Orchestrator.Backend.Domain.Ports.Configurator;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Maintainer;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Configurator
{
    [DomainService]
    public class TransformationService(
        ITransformationRepository<TransformationEntity> transformationRepository)
        : ITransformationService<TransformationEntity>

    {
        private readonly ITransformationRepository<TransformationEntity> _transformationRepository = transformationRepository;

        public async Task<IEnumerable<TransformationResponseModel>> GetAllAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(SynchronizationEntity.synchronization_code).Split("_")[1];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new TransformationSpecification(paginatedModel);
            return await _transformationRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new TransformationSpecification(paginatedModel);
            return await _transformationRepository.GetTotalRowsAsync(spec);
        }


       
    }
}
