using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administrations
{
    public class ConnectionService(
        IConnectionRepository<ConnectionEntity> connectionRepository) 
        : IConnectionService<ConnectionEntity>
    {
        private readonly IConnectionRepository<ConnectionEntity> _connectionRepository = connectionRepository;

        public async Task InsertAsync(ConnectionEntity connection)
        {
            await _connectionRepository.InsertAsync(connection);
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
    }
}
