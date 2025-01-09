using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator.Interfaces;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Models.Configurador.Catalog;
using Integration.Orchestrator.Backend.Domain.Ports.Configurator;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Maintainer;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Configurator
{
    [DomainService]
    public class CatalogService(
        ICatalogRepository<CatalogEntity> CatalogRepository,
        IAdapterRepository<AdapterEntity> adapterRepository,
        IEntitiesRepository<EntitiesEntity> entitiesRepository,
        IProcessRepository<ProcessEntity> processRepository,
        IPropertyRepository<PropertyEntity> propertyRepository,
        IRepositoryRepository<RepositoryEntity> repositoryRepository,
        IServerRepository<ServerEntity> serverRepository,
        IStatusService<StatusEntity> statusService)
        : ICatalogService<CatalogEntity>
    {
        private readonly ICatalogRepository<CatalogEntity> _catalogRepository = CatalogRepository;
        private readonly IAdapterRepository<AdapterEntity> _adapterRepository = adapterRepository;
        private readonly IEntitiesRepository<EntitiesEntity> _entitiesRepository = entitiesRepository;
        private readonly IProcessRepository<ProcessEntity> _processRepository = processRepository;
        private readonly IPropertyRepository<PropertyEntity> _propertyRepository = propertyRepository;
        private readonly IServerRepository<ServerEntity> _serverRepository = serverRepository;
        private readonly IRepositoryRepository<RepositoryEntity> _repositoryRepository = repositoryRepository;
        private readonly IStatusService<StatusEntity> _statusService = statusService;

        public async Task InsertAsync(CatalogEntity process)
        {
            await ValidateBussinesLogic(process, true);
            await _catalogRepository.InsertAsync(process);
        }

        public async Task UpdateAsync(CatalogEntity process)
        {
            await ValidateBussinesLogic(process);
            await _catalogRepository.UpdateAsync(process);
        }

        public async Task DeleteAsync(CatalogEntity process)
        {
            await _catalogRepository.DeleteAsync(process);
        }

        public async Task<CatalogEntity> GetByIdAsync(Guid id)
        {
            var specification = CatalogSpecification.GetByIdExpression(id);
            return await _catalogRepository.GetByIdAsync(specification);
        }

        public async Task<CatalogEntity> GetByCodeAsync(int code)
        {
            var specification = CatalogSpecification.GetByCodeExpression(code);
            return await _catalogRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<CatalogEntity>> GetByFatherAsync(int fatherCode)
        {
            var specification = CatalogSpecification.GetByFatherExpression(fatherCode);
            return await _catalogRepository.GetByFatherAsync(specification);
        }

        public async Task<IEnumerable<CatalogResponseModel>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(CatalogEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new CatalogSpecification(paginatedModel);
            return await _catalogRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new CatalogSpecification(paginatedModel);
            return await _catalogRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(CatalogEntity entity, bool create = false)
        {
            await EnsureStatusExists(entity.status_id);
            await ValidateCatalog(entity);
        }
        private async Task EnsureStatusExists(Guid statusId)
        {
            var statusFound = await _statusService.GetByIdAsync(statusId);
            if (statusFound == null)
            {
                throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors((int)ResponseCode.NotFoundSuccessfully, AppMessages.Application_StatusNotFound, statusId));
            }
        }
        private async Task ValidateCatalog(CatalogEntity entity)
        {

            var CodeExist = await _catalogRepository.GetByIdAsync(CatalogSpecification.GetByExpression(x => x.catalog_code == entity.catalog_code && x.id != entity.id)); 
 
            if (CodeExist!= null)
            {
                throw new OrchestratorArgumentException(string.Empty,
                   new DetailsArgumentErrors((int)ResponseCode.NotFoundSuccessfully,
                       string.Format(AppMessages.Domain_ResponseCode_Duplicate, "fatherCode","código"), entity));
            }

            if (!entity.is_father)
            {
                await ValidateInactiveFatherCatalog(entity);

                if (entity.father_code == null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                    new DetailsArgumentErrors((int)ResponseCode.NotFoundSuccessfully,
                        string.Format(AppMessages.Domain_ResponseCode_Requerired, "fatherCode"), entity));
                }
            }
            else
            {


                if (entity.father_code != null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                    new DetailsArgumentErrors((int)ResponseCode.NotFoundSuccessfully,
                        AppMessages.Domain_ResponseCode_Catalog_FatherCode_NoParent, entity));
                }
            }

            if (entity.is_father)
            {

                var CatalogName = await _catalogRepository.GetByIdAsync(x => x.catalog_name == entity.catalog_name && x.id != entity.id && x.is_father);
                if (CatalogName != null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                       new DetailsArgumentErrors((int)ResponseCode.NotFoundSuccessfully,
                           string.Format(AppMessages.Domain_ResponseCode_Duplicate, "name", "nombre"), entity));
                }
            }
            else
            {
                var CatalogName = await _catalogRepository.GetByIdAsync(x => x.catalog_name == entity.catalog_name && x.father_code==entity.father_code && x.is_father==false && x.id != entity.id );
                if (CatalogName != null)
                {
                    throw new OrchestratorArgumentException(string.Empty,
                       new DetailsArgumentErrors((int)ResponseCode.NotFoundSuccessfully,
                           string.Format(AppMessages.Domain_ResponseCode_Duplicate, "name", "nombre"), entity));
                }

            }


            
            await ValidateCatalogStatus(entity);
        }

        private async Task ValidateInactiveFatherCatalog(CatalogEntity entity)
        {
            var specification = CatalogSpecification.GetByFatherExpression(entity.catalog_code);
            var catalogFound = await _catalogRepository.GetByFatherAsync(specification);

            if (catalogFound != null && catalogFound.Any())
            {
                throw new OrchestratorArgumentException(string.Empty,
                    new DetailsArgumentErrors((int)ResponseCode.NotFoundSuccessfully,
                        AppMessages.Domain_ResponseCode_NotUpdateFatherInactiveDueToRelationship, entity));
            }
        }

        private async Task ValidateCatalogStatus(CatalogEntity entity)
        {

            if (!await _statusService.GetStatusIsActiveAsync(entity.status_id))
            {
                var relations = new List<Task<object>>
                {
                    _adapterRepository.GetByIdAsync(AdapterSpecification.GetByExpression(x => x.type_id == entity.id)).ContinueWith(t => (object)t.Result),
                    _entitiesRepository.GetByIdAsync(EntitiesSpecification.GetByExpression(x => x.type_id == entity.id)).ContinueWith(t => (object)t.Result),
                    _processRepository.GetByIdAsync(ProcessSpecification.GetByExpression(x => x.process_type_id == entity.id)).ContinueWith(t => (object)t.Result),
                    _propertyRepository.GetByIdAsync(PropertySpecification.GetByExpression(x => x.type_id == entity.id)).ContinueWith(t => (object)t.Result),
                    _serverRepository.GetByIdAsync(ServerSpecification.GetByExpression(x => x.type_id == entity.id)).ContinueWith(t => (object)t.Result),
                    _repositoryRepository.GetByIdAsync(RepositorySpecification.GetByExpression(x => x.auth_type_id == entity.id)).ContinueWith(t => (object)t.Result)
                };

                var results = await Task.WhenAll(relations);

                if (results.Any(result => result != null))
                {
                    throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors((int)ResponseCode.NotFoundSuccessfully,
                            AppMessages.Domain_ResponseCode_NotDeleteDueToRelationship, entity));
                }
            }
                
            
        }

    }
}
