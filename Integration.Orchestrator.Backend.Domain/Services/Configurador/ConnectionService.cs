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
    public class ConnectionService(
        IConnectionRepository<ConnectionEntity> connectionRepository,
        ICodeConfiguratorService codeConfiguratorService,
        IStatusService<StatusEntity> statusService) 
        : IConnectionService<ConnectionEntity>
    {
        private readonly IConnectionRepository<ConnectionEntity> _connectionRepository = connectionRepository;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;
        private readonly IStatusService<StatusEntity> _statusService = statusService;

        public async Task InsertAsync(ConnectionEntity connection)
        {
            await ValidateBussinesLogic(connection, true);
            await _connectionRepository.InsertAsync(connection);
        }

        public async Task UpdateAsync(ConnectionEntity connection)
        {
            await ValidateBussinesLogic(connection);
            await _connectionRepository.UpdateAsync(connection);
        }

        public async Task DeleteAsync(ConnectionEntity connection)
        {
            await _connectionRepository.DeleteAsync(connection);
        }

        public async Task<ConnectionEntity> GetByIdAsync(Guid id)
        {
            var specification = ConnectionSpecification.GetByIdExpression(id);
            return await _connectionRepository.GetByIdAsync(specification);
        }

        public async Task<ConnectionEntity> GetByServerIdAsync(Guid serverid, Guid idStatusActive)
        {
            var specification = ConnectionSpecification.GetByServerIdExpression(serverid, idStatusActive);
            return await _connectionRepository.GetByExpressionIdAsync(specification);
        }
        public async Task<ConnectionEntity> GetByAdapterIdAsync(Guid adapterid, Guid idStatusActive)
        {
            var specification = ConnectionSpecification.GetByAdapterIdExpression(adapterid, idStatusActive);
            return await _connectionRepository.GetByExpressionIdAsync(specification);
        }
        public async Task<ConnectionEntity> GetByRepositoryIdAsync(Guid repositoryid, Guid idStatusActive)
        {
            var specification = ConnectionSpecification.GetByRepositoryIdExpression(repositoryid, idStatusActive);
            return await _connectionRepository.GetByExpressionIdAsync(specification);
        }

        public async Task<ConnectionEntity> GetByCodeAsync(string code)
        {
            var specification = ConnectionSpecification.GetByCodeExpression(code);
            return await _connectionRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<ConnectionResponseModel>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(ConnectionEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new ConnectionSpecification(paginatedModel);

            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _connectionRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new ConnectionSpecification(paginatedModel);

            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _connectionRepository.GetTotalRows(spec);
        }

        private async Task<Expression<Func<ConnectionEntity, bool>>> ActiveStatusCriteria(Expression<Func<ConnectionEntity, bool>> criteria)
        {
            var entityFound = await _statusService.GetByKeyAsync(Status.active.ToString());
            return criteria = criteria.And(x => x.status_id == entityFound.id);
        }

        private async Task ValidateBussinesLogic(ConnectionEntity connection, bool create = false) 
        {
            await EnsureStatusExists(connection.status_id);

            if (create) 
            {
                var codeFound = await _codeConfiguratorService.GenerateCodeAsync(Prefix.Connection);
                await EnsureCodeIsUnique(codeFound);
                connection.connection_code = codeFound;
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
