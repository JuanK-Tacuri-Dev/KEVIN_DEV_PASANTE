using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administrations
{
    public class SynchronizationStatesService(
        ISynchronizationStatesRepository<SynchronizationStatesEntity> synchronizationStatesRepository) : ISynchronizationStatesService<SynchronizationStatesEntity>
    {
        private readonly ISynchronizationStatesRepository<SynchronizationStatesEntity> _synchronizationStatesRepository = synchronizationStatesRepository;

        public async Task InsertAsync(SynchronizationStatesEntity synchronization)
        {
            await _synchronizationStatesRepository.InsertAsync(synchronization);
        }

        public async Task<SynchronizationStatesEntity> GetByIdAsync(Guid id)
        {
            return await _synchronizationStatesRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<SynchronizationStatesEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new SynchronizationStatesSpecification(paginatedModel);
            return await _synchronizationStatesRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new SynchronizationStatesSpecification(paginatedModel);
            return await _synchronizationStatesRepository.GetTotalRows(spec);
        }
    }
}
