using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    public class SynchronizationStatesService(
        ISynchronizationStatesRepository<SynchronizationStatesEntity> synchronizationStatesStatesRepository) 
        : ISynchronizationStatesService<SynchronizationStatesEntity>
    {
        private readonly ISynchronizationStatesRepository<SynchronizationStatesEntity> _synchronizationStatesStatesRepository = synchronizationStatesStatesRepository;

        public async Task InsertAsync(SynchronizationStatesEntity synchronizationStatesStates)
        {
            await _synchronizationStatesStatesRepository.InsertAsync(synchronizationStatesStates);
        }

        public async Task UpdateAsync(SynchronizationStatesEntity synchronizationStates)
        {
            await _synchronizationStatesStatesRepository.UpdateAsync(synchronizationStates);
        }

        public async Task DeleteAsync(SynchronizationStatesEntity synchronizationStates)
        {
            await _synchronizationStatesStatesRepository.DeleteAsync(synchronizationStates);
        }

        public async Task<SynchronizationStatesEntity> GetByIdAsync(Guid id)
        {
            var specification = SynchronizationStatesSpecification.GetByIdExpression(id);
            return await _synchronizationStatesStatesRepository.GetByIdAsync(specification);
        }

        public async Task<IEnumerable<SynchronizationStatesEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new SynchronizationStatesSpecification(paginatedModel);
            return await _synchronizationStatesStatesRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new SynchronizationStatesSpecification(paginatedModel);
            return await _synchronizationStatesStatesRepository.GetTotalRows(spec);
        }
    }
}
