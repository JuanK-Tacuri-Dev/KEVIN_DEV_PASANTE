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
    public class IntegrationService(
        IIntegrationRepository<IntegrationEntity> integrationRepository,
        IStatusService<StatusEntity> statusService)
        : IIntegrationService<IntegrationEntity>
    {
        private readonly IIntegrationRepository<IntegrationEntity> _integrationRepository = integrationRepository;
        private readonly IStatusService<StatusEntity> _statusService = statusService;

        public async Task InsertAsync(IntegrationEntity integration)
        {
            await ValidateBussinesLogic(integration, true);
            await _integrationRepository.InsertAsync(integration);
        }

        public async Task UpdateAsync(IntegrationEntity integration)
        {
            await ValidateBussinesLogic(integration);
            await _integrationRepository.UpdateAsync(integration);
        }

        public async Task DeleteAsync(IntegrationEntity integration)
        {
            await _integrationRepository.DeleteAsync(integration);
        }
        public async Task<IntegrationEntity> GetByIdAsync(Guid id)
        {
            var specification = IntegrationSpecification.GetByIdExpression(id);
            return await _integrationRepository.GetByIdAsync(specification);
        }

        public async Task<IEnumerable<IntegrationEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(IntegrationEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new IntegrationSpecification(paginatedModel);
            return await _integrationRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new IntegrationSpecification(paginatedModel);
            return await _integrationRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(IntegrationEntity integration, bool create = false)
        {
            await EnsureStatusExists(integration.status_id);
            if (await validateProcessMinTwo(integration))
            {
                throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = AppMessages.Domain_IntegrationMinTwoRequired,
                            Data = integration
                        });
            }
        }

        private async Task<bool> validateProcessMinTwo(IntegrationEntity integration)
            => await Task.Run(() => integration.process.Count() < 2);

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
    }
}
