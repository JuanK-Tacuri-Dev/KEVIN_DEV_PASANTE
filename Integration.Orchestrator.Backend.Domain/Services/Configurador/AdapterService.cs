using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Configurador
{
    [DomainService]
    public class AdapterService(
        IAdapterRepository<AdapterEntity> adapterRepository,
        ICodeConfiguratorService codeConfiguratorService,
        IStatusService<StatusEntity> statusService)
        : IAdapterService<AdapterEntity>
    {
        private readonly IAdapterRepository<AdapterEntity> _adapterRepository = adapterRepository;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;
        private readonly IStatusService<StatusEntity> _statusService = statusService;
        public async Task InsertAsync(AdapterEntity adapter)
        {
            await ValidateBussinesLogic(adapter, true);
            await _adapterRepository.InsertAsync(adapter);
        }

        public async Task UpdateAsync(AdapterEntity adapter)
        {
            await ValidateBussinesLogic(adapter);
            await _adapterRepository.UpdateAsync(adapter);
        }

        public async Task DeleteAsync(AdapterEntity adapter)
        {
            await _adapterRepository.DeleteAsync(adapter);
        }

        public async Task<AdapterEntity> GetByIdAsync(Guid id)
        {
            var specification = AdapterSpecification.GetByIdExpression(id);
            return await _adapterRepository.GetByIdAsync(specification);
        }

        public async Task<AdapterEntity> GetByCodeAsync(string code)
        {
            var specification = AdapterSpecification.GetByCodeExpression(code);
            return await _adapterRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<AdapterEntity>> GetByTypeAsync(Guid typeId)
        {
            var specification = AdapterSpecification.GetByTypeExpression(typeId);
            return await _adapterRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<AdapterEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(AdapterEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new AdapterSpecification(paginatedModel);
            return await _adapterRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new AdapterSpecification(paginatedModel);
            return await _adapterRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(AdapterEntity adapter, bool create = false)
        {
            await EnsureStatusExists(adapter.status_id);
            await IsDuplicateVersionAndName(adapter);
            if (create)
            {
                var codeFound = await _codeConfiguratorService.GenerateCodeAsync(Prefix.Adapter);
                await EnsureCodeIsUnique(codeFound);
                adapter.adapter_code = codeFound;
            }
        }

        private async Task EnsureStatusExists(Guid statusId)
        {
            var statusFound = await _statusService.GetByIdAsync(statusId);
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

        private async Task IsDuplicateVersionAndName(AdapterEntity adapter)
        {
            var validateNameVersion = await _adapterRepository.ValidateAdapterNameVersion(adapter);
            if (validateNameVersion)
            {
                throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = AppMessages.Domain_AdapterExists
                        });
            }
        }
    }
}
