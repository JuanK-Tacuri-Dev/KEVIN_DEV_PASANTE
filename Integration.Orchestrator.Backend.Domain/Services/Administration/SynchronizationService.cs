using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
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
            var specification = SynchronizationSpecification.GetByIdExpression(id);
            return await _synchronizationRepository.GetByIdAsync(specification);
        }

        public async Task DeleteAsync(SynchronizationEntity synchronization)
        {
            await _synchronizationRepository.DeleteAsync(synchronization);
        }

        public async Task<IEnumerable<SynchronizationEntity>> GetByFranchiseIdAsync(Guid franchiseId)
        {
            var specification = SynchronizationSpecification.GetByFranchiseIdExpression(franchiseId);
            return await _synchronizationRepository.GetByFranchiseIdAsync(specification);
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
