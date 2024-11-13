using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Maintainer;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Configurador
{
    [DomainService]
    public class SynchronizationService(
        ISynchronizationRepository<SynchronizationEntity> synchronizationRepository,
        ICodeConfiguratorService codeConfiguratorService,
        ISynchronizationStatesService<SynchronizationStatusEntity> synchronizationStatesService) : 
        ISynchronizationService<SynchronizationEntity>
    {
        private readonly ISynchronizationRepository<SynchronizationEntity> _synchronizationRepository = synchronizationRepository;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;
        private readonly ISynchronizationStatesService<SynchronizationStatusEntity> _synchronizationStatesService = synchronizationStatesService;

        public async Task InsertAsync(SynchronizationEntity synchronization)
        {
            await ValidateBussinesLogic(synchronization, true);
            await _synchronizationRepository.InsertAsync(synchronization);
        }

        public async Task UpdateAsync(SynchronizationEntity synchronization)
        {
            await ValidateBussinesLogic(synchronization);
            await _synchronizationRepository.UpdateAsync(synchronization);
        }

        public async Task DeleteAsync(SynchronizationEntity synchronization)
        {
            await _synchronizationRepository.DeleteAsync(synchronization);
        }

        public async Task<SynchronizationEntity> GetByIdAsync(Guid id)
        {
            var specification = SynchronizationSpecification.GetByIdExpression(id);
            return await _synchronizationRepository.GetByIdAsync(specification);
        }

        public async Task<SynchronizationEntity> GetByCodeAsync(string code) 
        {
            var specification = SynchronizationSpecification.GetByCodeExpression(code);
            return await _synchronizationRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<SynchronizationEntity>> GetByFranchiseIdAsync(Guid franchiseId)
        {
            var specification = SynchronizationSpecification.GetByFranchiseIdExpression(franchiseId);
            return await _synchronizationRepository.GetByFranchiseIdAsync(specification);
        }

        public async Task<IEnumerable<SynchronizationEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(SynchronizationEntity.synchronization_code).Split("_")[1];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new SynchronizationSpecification(paginatedModel);
            return await _synchronizationRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new SynchronizationSpecification(paginatedModel);
            return await _synchronizationRepository.GetTotalRows(spec);
        }

        public async Task<SynchronizationEntity> GetByIntegrationIdAsync(Guid idIntegration, Guid IdStatusCanceled)
        {
            var specification = SynchronizationSpecification.GetByIntegrationIdExpression(idIntegration, IdStatusCanceled);
            return await _synchronizationRepository.GetByIdAsync(specification);
        }

        private async Task ValidateBussinesLogic(SynchronizationEntity synchronization, bool create = false)
        {
            await EnsureStatusExists(synchronization.status_id);

            if (create)
            {
                var codeFound = await _codeConfiguratorService.GenerateCodeAsync(Prefix.Synchronyzation);
                await EnsureCodeIsUnique(codeFound);
                synchronization.synchronization_code = codeFound;
            }
        }
        private async Task EnsureStatusExists(Guid statusId)
        {
            var statusFound = await _synchronizationStatesService.GetByIdAsync(statusId);
            if (statusFound == null)
            {
                throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = AppMessages.Application_StatusNotFound,
                            Data = statusId
                        });
            }
        }

        private async Task EnsureCodeIsUnique(string code)
        {
            var codeFound = await GetByCodeAsync(code);
            if (codeFound != null)
            {
                throw new OrchestratorArgumentException(string.Empty,
                    new DetailsArgumentErrors()
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = AppMessages.Domain_Response_CodeInUse,
                        Data = code
                    });
            }
        }

       
    }
}
