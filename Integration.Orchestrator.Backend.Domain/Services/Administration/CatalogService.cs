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
    public class CatalogService(
        ICatalogRepository<CatalogEntity> processRepository,
        IStatusService<StatusEntity> statusService)
        : ICatalogService<CatalogEntity>
    {
        private readonly ICatalogRepository<CatalogEntity> _processRepository = processRepository;
        private readonly IStatusService<StatusEntity> _statusService = statusService;

        public async Task InsertAsync(CatalogEntity process)
        {
            await ValidateBussinesLogic(process, true);
            await _processRepository.InsertAsync(process);
        }

        public async Task UpdateAsync(CatalogEntity process)
        {
            await ValidateBussinesLogic(process);
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

        public async Task<CatalogEntity> GetByCodeAsync(int code)
        {
            var specification = CatalogSpecification.GetByCodeExpression(code);
            return await _processRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<CatalogEntity>> GetByFatherAsync(int fatherCode)
        {
            var specification = CatalogSpecification.GetByFatherExpression(fatherCode);
            return await _processRepository.GetByFatherAsync(specification);
        }

        public async Task<IEnumerable<CatalogEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(CatalogEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new CatalogSpecification(paginatedModel);
            return await _processRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new CatalogSpecification(paginatedModel);
            return await _processRepository.GetTotalRows(spec);
        }
        
        private async Task ValidateBussinesLogic(CatalogEntity entity, bool create = false)
        {
            await EnsureStatusExists(entity.status_id);
            if (create) 
            {
                var catalogList = await GetByNameAndFatherCodeAsync(entity.catalog_name, entity.father_code);

                switch (create)
                {
                    case true when catalogList.ToList().Count > 0:
                        throw new ArgumentException(AppMessages.Domain_CatalogFatherCodeExists);
                    case false when catalogList.ToList().Exists(catalogEntity => catalogEntity.id != entity.id):
                        throw new ArgumentException(AppMessages.Domain_CatalogFatherCodeExists);
                }
            }
            
        }
        
        public async Task<IEnumerable<CatalogEntity>> GetByNameAndFatherCodeAsync(string name, int? fatherCode)
        {
            var specification = CatalogSpecification.GetByNameAndFatherCodeExpression(name, fatherCode);
            return await _processRepository.GetByNameAndFatherCodeAsync(specification);
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
    }
}
