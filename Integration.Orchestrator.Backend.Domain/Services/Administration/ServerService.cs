using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    [DomainService]
    public class ServerService(
        IServerRepository<ServerEntity> serverRepository) 
        : IServerService<ServerEntity>
    {
        private readonly IServerRepository<ServerEntity> _serverRepository = serverRepository;

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

        public async Task<IEnumerable<ServerEntity>> GetByTypeAsync(string type)
        {
            var specification = ServerSpecification.GetByTypeExpression(type);
            return await _serverRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<ServerEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
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
            if (create) 
            {
                var serverByCode = await GetByCodeAsync(server.server_code);
                if (serverByCode != null) 
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = AppMessages.Domain_Response_CodeInUse
                        });
                }
            }
        }
    }
}
