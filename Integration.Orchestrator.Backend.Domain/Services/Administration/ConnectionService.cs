﻿using Integration.Orchestrator.Backend.Domain.Commons;
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
    public class ConnectionService(
        IConnectionRepository<ConnectionEntity> connectionRepository) 
        : IConnectionService<ConnectionEntity>
    {
        private readonly IConnectionRepository<ConnectionEntity> _connectionRepository = connectionRepository;

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

        public async Task<ConnectionEntity> GetByCodeAsync(string code)
        {
            var specification = ConnectionSpecification.GetByCodeExpression(code);
            return await _connectionRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<ConnectionEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(ConnectionEntity.created_at);
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new ConnectionSpecification(paginatedModel);
            return await _connectionRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new ConnectionSpecification(paginatedModel);
            return await _connectionRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(ConnectionEntity connection, bool create = false) 
        {
            if (create) 
            {
                var connectionByCode = await GetByCodeAsync(connection.connection_code);
                if (connectionByCode != null) 
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
