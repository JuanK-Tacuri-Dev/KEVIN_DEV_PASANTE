using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    [DomainService]
    public class EntitiesService(
        IEntitiesRepository<EntitiesEntity> entitiesRepository) 
        : IEntitiesService<EntitiesEntity>
    {
        private readonly IEntitiesRepository<EntitiesEntity> _entitiesRepository = entitiesRepository;

        public async Task InsertAsync(EntitiesEntity entities)
        {
            await ValidateBussinesLogic(entities, true);
            await _entitiesRepository.InsertAsync(entities);
        }

        public async Task UpdateAsync(EntitiesEntity entities)
        {
            await ValidateBussinesLogic(entities);
            await _entitiesRepository.UpdateAsync(entities);
        }

        public async Task DeleteAsync(EntitiesEntity entities)
        {
            await _entitiesRepository.DeleteAsync(entities);
        }

        public async Task<EntitiesEntity> GetByIdAsync(Guid id)
        {
            var specification = EntitiesSpecification.GetByIdExpression(id);
            return await _entitiesRepository.GetByIdAsync(specification);
        }

        public async Task<EntitiesEntity> GetByCodeAsync(string code)
        {
            var specification = EntitiesSpecification.GetByCodeExpression(code);
            return await _entitiesRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<EntitiesEntity>> GetByTypeIdAsync(Guid typeId)
        {
            var specification = EntitiesSpecification.GetByTypeExpression(typeId);
            return await _entitiesRepository.GetByTypeIdAsync(specification);
        }

        public async Task<IEnumerable<EntitiesEntity>> GetByRepositoryIdAsync(Guid repositoryId)
        {
            var specification = EntitiesSpecification.GetByRepositoryIdExpression(repositoryId);
            var byRepositoryIdAsync = _entitiesRepository.GetByRepositoryIdAsync(specification);

            return await byRepositoryIdAsync;
        }

        public async Task<IEnumerable<EntitiesEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new EntitiesSpecification(paginatedModel);
            return await _entitiesRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new EntitiesSpecification(paginatedModel);
            return await _entitiesRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(EntitiesEntity entities, bool create = false) 
        {
            if (create)
            {
                var entitiesByCode = await GetByCodeAsync(entities.entity_code);
                var numEntitiesByNameAndRepositoryId = await CountEntitiesByNameAndRepositoryIdAsync(entities);

                if (entitiesByCode != null)
                {
                    throw new ArgumentException(AppMessages.Domain_EntitiesExists);
                }
                
                if (numEntitiesByNameAndRepositoryId > 0)
                {
                    throw new ArgumentException(AppMessages.Domain_EntityRepositoryExists);
                }
            }
        }

        private async Task<int> CountEntitiesByNameAndRepositoryIdAsync(EntitiesEntity entity)
        {
            var entitiesByNameAndRepositoryId = await GetByNameAndRepositoryIdAsync(entity.entity_name, entity.repository_id);
            return entitiesByNameAndRepositoryId.ToList().Count;
        }

        public async Task<IEnumerable<EntitiesEntity>> GetByNameAndRepositoryIdAsync(string name, Guid repositoryId)
        {
            var specification = EntitiesSpecification.GetByNameAndRepositoryIdExpression(name, repositoryId);
            return await _entitiesRepository.GetByNameAndRepositoryIdAsync(specification);
        }
    }
}
