using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    public class ProcessService(
        IProcessRepository<ProcessEntity> connectionRepository) 
        : IProcessService<ProcessEntity>
    {
        private readonly IProcessRepository<ProcessEntity> _connectionRepository = connectionRepository;

        public async Task InsertAsync(ProcessEntity connection)
        {
            await ValidateBussinesLogic(connection, true);
            await _connectionRepository.InsertAsync(connection);
        }

        public async Task<ProcessEntity> GetByCodeAsync(string code)
        {
            var specification = ProcessSpecification.GetByCodeExpression(code);
            return await _connectionRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<ProcessEntity>> GetByTypeAsync(string type)
        {
            var specification = ProcessSpecification.GetByTypeExpression(type);
            return await _connectionRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<ProcessEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new ProcessSpecification(paginatedModel);
            return await _connectionRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new ProcessSpecification(paginatedModel);
            return await _connectionRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(ProcessEntity connection, bool create = false) 
        {
            if (create) 
            {
                var connectionByCode = await GetByCodeAsync(connection.process_code);
                if (connectionByCode != null) 
                {
                    throw new ArgumentException(AppMessages.Domain_ProcessExists);
                }
            }
        }
    }
}
