﻿using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    public class ConnectionService(
        IConnectionRepository<ConnectionEntity> connectionRepository) 
        : IConnectionService<ConnectionEntity>
    {
        private readonly IConnectionRepository<ConnectionEntity> _connectionRepository = connectionRepository;

        public async Task InsertAsync(ConnectionEntity connetion)
        {
            await ValidateBussinesLogic(connetion, true);
            await _connectionRepository.InsertAsync(connetion);
        }

        public async Task UpdateAsync(ConnectionEntity connetion)
        {
            await ValidateBussinesLogic(connetion);
            await _connectionRepository.UpdateAsync(connetion);
        }

        public async Task DeleteAsync(ConnectionEntity connetion)
        {
            await _connectionRepository.DeleteAsync(connetion);
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

        public async Task<IEnumerable<ConnectionEntity>> GetByTypeAsync(string type)
        {
            var specification = ConnectionSpecification.GetByTypeExpression(type);
            return await _connectionRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<ConnectionEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new ConnectionSpecification(paginatedModel);
            return await _connectionRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new ConnectionSpecification(paginatedModel);
            return await _connectionRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(ConnectionEntity connetion, bool create = false) 
        {
            if (create) 
            {
                var connectionByCode = await GetByCodeAsync(connetion.connection_code);
                if (connectionByCode != null) 
                {
                    throw new ArgumentException(AppMessages.Domain_ConnectionExists);
                }
            }
        }
    }
}
