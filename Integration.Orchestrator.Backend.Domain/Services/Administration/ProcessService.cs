using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    public class ProcessService(
        IProcessRepository<ProcessEntity> processRepository)
        : IProcessService<ProcessEntity>
    {
        private readonly IProcessRepository<ProcessEntity> _processRepository = processRepository;

        public async Task InsertAsync(ProcessEntity process)
        {
            await ValidateBussinesLogic(process, true);
            await _processRepository.InsertAsync(process);
        }

        public async Task UpdateAsync(ProcessEntity process)
        {
            await _processRepository.UpdateAsync(process);
        }

        public async Task DeleteAsync(ProcessEntity process)
        {
            await _processRepository.DeleteAsync(process);
        }

        public async Task<ProcessEntity> GetByIdAsync(Guid id)
        {
            var specification = ProcessSpecification.GetByIdExpression(id);
            return await _processRepository.GetByIdAsync(specification);
        }

        public async Task<ProcessEntity> GetByCodeAsync(string code)
        {
            var specification = ProcessSpecification.GetByCodeExpression(code);
            return await _processRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<ProcessEntity>> GetByTypeAsync(string type)
        {
            var specification = ProcessSpecification.GetByTypeExpression(type);
            return await _processRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<ProcessEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new ProcessSpecification(paginatedModel);
            return await _processRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new ProcessSpecification(paginatedModel);
            return await _processRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(ProcessEntity process, bool create = false)
        {
            if (create)
            {
                var processByCode = await GetByCodeAsync(process.process_code);
                if (processByCode != null)
                {
                    throw new ArgumentException(AppMessages.Domain_ProcessExists);
                }
            }
        }
    }
}
