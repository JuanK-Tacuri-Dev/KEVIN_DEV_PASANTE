using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
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
            }
        }
    }
}
