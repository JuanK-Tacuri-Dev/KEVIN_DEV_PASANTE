using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    [DomainService]
    public class IntegrationService(
        IIntegrationRepository<IntegrationEntity> integrationRepository)
        : IIntegrationService<IntegrationEntity>
    {
        private readonly IIntegrationRepository<IntegrationEntity> _integrationRepository = integrationRepository;

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
                paginatedModel.Sort_field = nameof(IntegrationEntity.created_at);
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
            if (await validateProcessMinTwo(integration))
            {
                throw new ArgumentException(AppMessages.Domain_IntegrationMinTwoRequired);
            }
        }

        private async Task<bool> validateProcessMinTwo(IntegrationEntity integration)
            => await Task.Run(() => integration.process.Count() < 2);
    }
}
