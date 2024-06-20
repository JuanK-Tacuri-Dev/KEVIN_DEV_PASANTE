using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    public class StatusService(
        IStatusRepository<StatusEntity> connectionRepository) 
        : IStatusService<StatusEntity>
    {
        private readonly IStatusRepository<StatusEntity> _connectionRepository = connectionRepository;

        public async Task InsertAsync(StatusEntity connection)
        {
            await ValidateBussinesLogic(connection, true);
            await _connectionRepository.InsertAsync(connection);
        }

        public async Task<IEnumerable<StatusEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new StatusSpecification(paginatedModel);
            return await _connectionRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new StatusSpecification(paginatedModel);
            return await _connectionRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(StatusEntity connection, bool create = false) 
        {
            if (create) 
            {
            }
        }
    }
}
