using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Dto.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Server;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Maintainer;
using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Services.Configurador
{
    [DomainService]
    public class ServerService(
        IServerRepository<ServerEntity> serverRepository,
        ICodeConfiguratorService codeConfiguratorService,
        IStatusService<StatusEntity> statusService)
        : IServerService<ServerEntity>
    {
        private readonly IServerRepository<ServerEntity> _serverRepository = serverRepository;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;
        private readonly IStatusService<StatusEntity> _statusService = statusService;

        public async Task InsertAsync(ServerEntity server)
        {
            await ValidateBussinesLogic(server, true);
            await _serverRepository.InsertAsync(server);
        }

        public async Task UpdateAsync(ServerEntity server)
        {
            await ValidateBussinesLogic(server);
            await _serverRepository.UpdateAsync(server);
        }

        public async Task DeleteAsync(ServerEntity server)
        {
            await _serverRepository.DeleteAsync(server);
        }

        public async Task<ServerEntity> GetByIdAsync(Guid id)
        {
            var specification = ServerSpecification.GetByIdExpression(id);
            return await _serverRepository.GetByIdAsync(specification);
        }

        public async Task<ServerEntity> GetByCodeAsync(string code)
        {
            var specification = ServerSpecification.GetByCodeExpression(code);
            return await _serverRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<ServerEntity>> GetByTypeAsync(Guid typeId)
        {
            var specification = ServerSpecification.GetByTypeExpression(typeId);
            return await _serverRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<ServerDto>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(ServerEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new ServerSpecification(paginatedModel);

            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _serverRepository.GetAllAsync(spec);
        }
        public async Task<IEnumerable<ServerResponseTest>> GetAllPaginatedAsyncTest(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(ServerEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new ServerSpecification(paginatedModel);

            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _serverRepository.GetAllAsyncTest(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new ServerSpecification(paginatedModel);

            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _serverRepository.GetTotalRows(spec);
        }

        private async Task<Expression<Func<ServerEntity, bool>>> ActiveStatusCriteria(Expression<Func<ServerEntity, bool>> criteria)
        {
            var entityFound = await _statusService.GetByKeyAsync(Status.active.ToString());
            return criteria = criteria.And(x => x.status_id == entityFound.id);
        }

        private async Task ValidateBussinesLogic(ServerEntity server, bool create = false)
        {
            await EnsureStatusExists(server.status_id);
            await IsDuplicateNameAndUrl(server);
            if (create)
            {
                var codeFound = await _codeConfiguratorService.GenerateCodeAsync(Prefix.Server);
                await EnsureCodeIsUnique(codeFound);
                server.server_code = codeFound;
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

        private async Task IsDuplicateNameAndUrl(ServerEntity server)
        {
            var validateNameURL = await _serverRepository.ValidateNameURL(server);
            if (validateNameURL)
            {
                throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = AppMessages.Domain_ServerExists,
                            Data = server
                        });
            }
        }
    }
}
