using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization;
using Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Services.Adminitrations
{
    public class SynchronizationService(ISynchronizationRepository<SynchronizationEntity> synchronizationRepository) : ISynchronizationService<SynchronizationEntity>
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

        public Task<IEnumerable<SynchronizationEntity>> GetByFranchiseIdAsync(Guid franchiseId)
        {
            return _synchronizationRepository.GetByFranchiseIdAsync(franchiseId);
        }

        public Task<IEnumerable<SynchronizationEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new SynchronizationSpecification(paginatedModel);
            return _synchronizationRepository.GetAllAsync(spec);
        }

        public Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new SynchronizationSpecification(paginatedModel);
            return _synchronizationRepository.GetTotalRows(spec);
        }
    }
}
