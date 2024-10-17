using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
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

        public async Task<IEnumerable<ServerEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(ServerEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new ServerSpecification(paginatedModel);
            return await _serverRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new ServerSpecification(paginatedModel);
            return await _serverRepository.GetTotalRows(spec);
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
                            Description = AppMessages.Domain_ServerExists
                        });
            }
        }
    }
}
