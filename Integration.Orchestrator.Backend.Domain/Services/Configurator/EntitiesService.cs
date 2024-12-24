using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurator.Entity;
using Integration.Orchestrator.Backend.Domain.Ports.Configurator;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Maintainer;
using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Services.Configurator
{
    [DomainService]
    public class EntitiesService(
        IEntitiesRepository<EntitiesEntity> entitiesRepository,
        ICodeConfiguratorService codeConfiguratorService,
        IStatusService<StatusEntity> statusService)
        : IEntitiesService<EntitiesEntity>
    {
        private readonly IEntitiesRepository<EntitiesEntity> _entitiesRepository = entitiesRepository;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;
        private readonly IStatusService<StatusEntity> _statusService = statusService;

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

        public async Task<IEnumerable<EntitiesEntity>> GetByRepositoryIdAsync(Guid repositoryId, Guid idStatusActive)
        {
            var specification = EntitiesSpecification.GetByRepositoryIdExpression(repositoryId, idStatusActive);
            var byRepositoryIdAsync = _entitiesRepository.GetByRepositoryIdAsync(specification);

            return await byRepositoryIdAsync;
        }

        public async Task<IEnumerable<EntityResponseModel>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(EntitiesEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new EntitiesSpecification(paginatedModel);
            
            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _entitiesRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new EntitiesSpecification(paginatedModel);

            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _entitiesRepository.GetTotalRows(spec);
        }

        private async Task<Expression<Func<EntitiesEntity, bool>>> ActiveStatusCriteria(Expression<Func<EntitiesEntity, bool>> criteria)
        {
            var entityFound = await _statusService.GetByKeyAsync(Status.active.ToString());
            return criteria = criteria.And(x => x.status_id == entityFound.id);
        }

        private async Task ValidateBussinesLogic(EntitiesEntity entities, bool create = false)
        {
            await EnsureStatusExists(entities.status_id);
            await IsDuplicateRepositoryAndName(entities);
            if (create)
            {
                var codeFound = await _codeConfiguratorService.GenerateCodeAsync(Prefix.Entity);
                await EnsureCodeIsUnique(codeFound);
                entities.entity_code = codeFound;
            }
        }

        private async Task EnsureStatusExists(Guid statusId)
        {
            var statusFound = await _statusService.GetByIdAsync(statusId);
            if (statusFound == null)
            {
                throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = AppMessages.Application_StatusNotFound,
                            Data = statusId
                        });
            }
        }

        private async Task EnsureCodeIsUnique(string code)
        {
            var codeFound = await GetByCodeAsync(code);
            if (codeFound != null)
            {
                throw new OrchestratorArgumentException(string.Empty,
                    new DetailsArgumentErrors()
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = AppMessages.Domain_Response_CodeInUse,
                        Data = code
                    });
            }
        }
        private async Task IsDuplicateRepositoryAndName(EntitiesEntity entity)
        {
            var exits = await _entitiesRepository.GetRepositoryAndNameExists(entity);
            if (exits)
            {
                throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = AppMessages.Domain_EntitiesExists,
                            Data = entity
                        });
            }
        }
    }
}
