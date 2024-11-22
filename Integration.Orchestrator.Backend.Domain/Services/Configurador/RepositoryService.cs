using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurador;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Maintainer;
using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Services.Configurador
{
    [DomainService]
    public class RepositoryService(
        IRepositoryRepository<RepositoryEntity> repositoryRepository,
         ICodeConfiguratorService codeConfiguratorService,
        IStatusService<StatusEntity> statusService)
        : IRepositoryService<RepositoryEntity>
    {
        private readonly IRepositoryRepository<RepositoryEntity> _repositoryRepository = repositoryRepository;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;
        private readonly IStatusService<StatusEntity> _statusService = statusService;

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

        public async Task<IEnumerable<RepositoryResponseModel>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(RepositoryEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new RepositorySpecification(paginatedModel);

            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _repositoryRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new RepositorySpecification(paginatedModel);

            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _repositoryRepository.GetTotalRows(spec);
        }

        private async Task<Expression<Func<RepositoryEntity, bool>>> ActiveStatusCriteria(Expression<Func<RepositoryEntity, bool>> criteria)
        {
            var entityFound = await _statusService.GetByKeyAsync(Status.active.ToString());
            return criteria = criteria.And(x => x.status_id == entityFound.id);
        }

        private async Task ValidateBussinesLogic(RepositoryEntity repository, bool create = false)
        {
            await EnsureStatusExists(repository.status_id);
            await IsDuplicateDbPortUser(repository);
            if (create)
            {
                var codeFound = await _codeConfiguratorService.GenerateCodeAsync(Prefix.Repository);
                await EnsureCodeIsUnique(codeFound);
                repository.repository_code = codeFound;
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

        private async Task IsDuplicateDbPortUser(RepositoryEntity repository)
        {
            var validateDbPortUser = await _repositoryRepository.ValidateDbPortUser(repository);
            if (validateDbPortUser)
            {
                throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = AppMessages.Domain_RepositoryExists,
                            Data = repository
                        });
            }
        }
    }
}
