using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Maintainer;
using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Services.Configurador
{
    [DomainService]
    public class ProcessService(
        IProcessRepository<ProcessEntity> processRepository,
         ICodeConfiguratorService codeConfiguratorService,
        IStatusService<StatusEntity> statusService)
        : IProcessService<ProcessEntity>
    {
        private readonly IProcessRepository<ProcessEntity> _processRepository = processRepository;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;
        private readonly IStatusService<StatusEntity> _statusService = statusService;

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

        public async Task<IEnumerable<ProcessEntity>> GetByTypeAsync(Guid typeId)
        {
            var specification = ProcessSpecification.GetByTypeExpression(typeId);
            return await _processRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<ProcessEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(ProcessEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }

            var spec = new ProcessSpecification(paginatedModel);
            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _processRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            
            var spec = new ProcessSpecification(paginatedModel);
            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _processRepository.GetTotalRows(spec);
        }

        private async Task<Expression<Func<ProcessEntity, bool>>> ActiveStatusCriteria(Expression<Func<ProcessEntity, bool>> criteria)
        {
            var entityFound = await _statusService.GetByKeyAsync(Status.active.ToString());
            return criteria = criteria.And(x => x.status_id == entityFound.id);
        }

        private async Task ValidateBussinesLogic(ProcessEntity process, bool create = false)
        {
            await EnsureStatusExists(process.status_id);

            if (create)
            {
                var codeFound = await _codeConfiguratorService.GenerateCodeAsync(Prefix.Process);
                await EnsureCodeIsUnique(codeFound);
                process.process_code = codeFound;
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
    }
}
