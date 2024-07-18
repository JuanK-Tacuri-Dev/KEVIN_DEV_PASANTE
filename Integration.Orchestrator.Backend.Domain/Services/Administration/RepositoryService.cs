using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    [DomainService]
    public class RepositoryService(
        IRepositoryRepository<RepositoryEntity> repositoryRepository) 
        : IRepositoryService<RepositoryEntity>
    {
        private readonly IRepositoryRepository<RepositoryEntity> _repositoryRepository = repositoryRepository;

        public async Task InsertAsync(RepositoryEntity repository)
        {
            await ValidateBussinesLogic(repository, true);
            await _repositoryRepository.InsertAsync(repository);
        }

        public async Task UpdateAsync(RepositoryEntity repository)
        {
            await ValidateBussinesLogic(repository);
            await _repositoryRepository.UpdateAsync(repository);
        }

        public async Task DeleteAsync(RepositoryEntity repository)
        {
            await _repositoryRepository.DeleteAsync(repository);
        }

        public async Task<RepositoryEntity> GetByIdAsync(Guid id)
        {
            var specification = RepositorySpecification.GetByIdExpression(id);
            return await _repositoryRepository.GetByIdAsync(specification);
        }

        public async Task<RepositoryEntity> GetByCodeAsync(string code)
        {
            var specification = RepositorySpecification.GetByCodeExpression(code);
            return await _repositoryRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<RepositoryEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new RepositorySpecification(paginatedModel);
            return await _repositoryRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new RepositorySpecification(paginatedModel);
            return await _repositoryRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(RepositoryEntity repository, bool create = false) 
        {
            if (create)
            {
                var repositoryByCode = await GetByCodeAsync(repository.repository_code);
                if (repositoryByCode != null) 
                {
                    throw new ArgumentException(AppMessages.Domain_RepositoryExists);
                }
            }
        }
    }
}
