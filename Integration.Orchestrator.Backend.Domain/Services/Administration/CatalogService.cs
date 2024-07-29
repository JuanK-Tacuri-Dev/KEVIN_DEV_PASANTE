using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    [DomainService]
    public class CatalogService(
        ICatalogRepository<CatalogEntity> processRepository)
        : ICatalogService<CatalogEntity>
    {
        private readonly ICatalogRepository<CatalogEntity> _processRepository = processRepository;

        public async Task InsertAsync(CatalogEntity process)
        {
            await _processRepository.InsertAsync(process);
        }

        public async Task UpdateAsync(CatalogEntity process)
        {
            await _processRepository.UpdateAsync(process);
        }

        public async Task DeleteAsync(CatalogEntity process)
        {
            await _processRepository.DeleteAsync(process);
        }

        public async Task<CatalogEntity> GetByIdAsync(Guid id)
        {
            var specification = CatalogSpecification.GetByIdExpression(id);
            return await _processRepository.GetByIdAsync(specification);
        }

        public async Task<IEnumerable<CatalogEntity>> GetByTypeAsync(string type)
        {
            var specification = CatalogSpecification.GetByTypeExpression(type);
            return await _processRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<CatalogEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new CatalogSpecification(paginatedModel);
            return await _processRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new CatalogSpecification(paginatedModel);
            return await _processRepository.GetTotalRows(spec);
        }
    }
}
