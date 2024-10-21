using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Maintainer;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Configurador
{
    [DomainService]
    public class SynchronizationStatesService(
        ISynchronizationStatesRepository<SynchronizationStatusEntity> synchronizationStatesStatesRepository) 
        : ISynchronizationStatesService<SynchronizationStatusEntity>
    {
        private readonly ISynchronizationStatesRepository<SynchronizationStatusEntity> _synchronizationStatesStatesRepository = synchronizationStatesStatesRepository;

        public async Task InsertAsync(SynchronizationStatusEntity synchronizationStates)
        {
            await ValidateBussinesLogic(synchronizationStates, true);
            await _synchronizationStatesStatesRepository.InsertAsync(synchronizationStates);
        }

        public async Task UpdateAsync(SynchronizationStatusEntity synchronizationStates)
        {
            await ValidateBussinesLogic(synchronizationStates);
            await _synchronizationStatesStatesRepository.UpdateAsync(synchronizationStates);
        }

        public async Task DeleteAsync(SynchronizationStatusEntity synchronizationStates)
        {
            await _synchronizationStatesStatesRepository.DeleteAsync(synchronizationStates);
        }

        public async Task<SynchronizationStatusEntity> GetByIdAsync(Guid id)
        {
            var specification = SynchronizationStatesSpecification.GetByIdExpression(id);
            return await _synchronizationStatesStatesRepository.GetByIdAsync(specification);
        }

        public async Task<SynchronizationStatusEntity> GetByCodeAsync(string code)
        {
            var specification = SynchronizationStatesSpecification.GetByCodeExpression(code);
            return await _synchronizationStatesStatesRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<SynchronizationStatusEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(SynchronizationStatusEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new SynchronizationStatesSpecification(paginatedModel);
            return await _synchronizationStatesStatesRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new SynchronizationStatesSpecification(paginatedModel);
            return await _synchronizationStatesStatesRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(SynchronizationStatusEntity synchronizationStatesEntity, bool create = false)
        {
            if (create)
            {
                var codeFound = await GetByCodeAsync(synchronizationStatesEntity.synchronization_status_key);
                if (codeFound != null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = AppMessages.Domain_Response_CodeInUse,
                            Data = synchronizationStatesEntity
                        });
                }
            }
        }
    }
}
