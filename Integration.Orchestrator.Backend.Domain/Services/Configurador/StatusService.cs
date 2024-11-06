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
    public class StatusService(
        IStatusRepository<StatusEntity> statusRepository)
        : IStatusService<StatusEntity>
    {
        private readonly IStatusRepository<StatusEntity> _statusRepository = statusRepository;

        public async Task InsertAsync(StatusEntity status)
        {
            await ValidateBussinesLogic(status, true);
            await _statusRepository.InsertAsync(status);
        }

        public async Task UpdateAsync(StatusEntity status)
        {
            await ValidateBussinesLogic(status);
            await _statusRepository.UpdateAsync(status);
        }

        public async Task DeleteAsync(StatusEntity status)
        {
            await _statusRepository.DeleteAsync(status);
        }

        public async Task<StatusEntity> GetByIdAsync(Guid id)
        {
            var specification = StatusSpecification.GetByIdExpression(id);
            return await _statusRepository.GetByIdAsync(specification);
        }

        public async Task<bool> GetStatusIsActive(Guid id)
        {
            var specification = StatusSpecification.GetStatusIsActive(id, "active");
            return await _statusRepository.GetStatusIsActive(specification);
        }
        public async Task<StatusEntity> GetByKeyAsync(string key)
        {
            var specification = StatusSpecification.GetByCodeExpression(key);
            return await _statusRepository.GetByKeyAsync(specification);
        }

        public async Task<IEnumerable<StatusEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(StatusEntity.created_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new StatusSpecification(paginatedModel);
            return await _statusRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new StatusSpecification(paginatedModel);
            return await _statusRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(StatusEntity status, bool create = false)
        {
            if (create)
            {
                var processByType = await GetByKeyAsync(status.status_key);
                if (processByType != null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = AppMessages.Domain_Response_CodeInUse,
                            Data = status
                        });
                }
            }
        }
    }
}
