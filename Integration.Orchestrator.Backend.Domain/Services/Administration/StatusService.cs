using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
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

        public async Task<StatusEntity> GetByCodeAsync(string code)
        {
            var specification = StatusSpecification.GetByCodeExpression(code);
            return await _statusRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<StatusEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
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
                var processByCode = await GetByCodeAsync(status.key);
                if (processByCode != null)
                {
                    throw new ArgumentException(AppMessages.Application_ResponseNotCreated);
                }
            }
        }
    }
}
