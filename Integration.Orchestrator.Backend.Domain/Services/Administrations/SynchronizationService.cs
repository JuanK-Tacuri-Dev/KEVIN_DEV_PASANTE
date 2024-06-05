using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administrations
{
    public class SynchronizationService(
        ISynchronizationRepository<SynchronizationEntity> synchronizationRepository) : ISynchronizationService<SynchronizationEntity>
    {
        private readonly ISynchronizationRepository<SynchronizationEntity> _synchronizationRepository = synchronizationRepository;

        public async Task InsertAsync(SynchronizationEntity synchronization)
        {
            await _synchronizationRepository.InsertAsync(synchronization);
        }

        public async Task UpdateAsync(SynchronizationEntity synchronization)
        {
            await _synchronizationRepository.UpdateAsync(synchronization);
        }

        public async Task<SynchronizationEntity> GetByIdAsync(Guid id)
        {
            return await _synchronizationRepository.GetByIdAsync(id);
        }

        public async Task DeleteAsync(SynchronizationEntity synchronization)
        {
            await _synchronizationRepository.DeleteAsync(synchronization);
        }

        public async Task<IEnumerable<SynchronizationEntity>> GetByFranchiseIdAsync(Guid franchiseId)
        {
            return await _synchronizationRepository.GetByFranchiseIdAsync(franchiseId);
        }

        public async Task<string> GetStatusByIdAsync(Guid idStatus) 
        {
            return await Task.Run(() => "");
        }

        public async Task<IEnumerable<SynchronizationEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new SynchronizationSpecification(paginatedModel);
            return await _synchronizationRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new SynchronizationSpecification(paginatedModel);
            return await _synchronizationRepository.GetTotalRows(spec);
        }
    }
}
