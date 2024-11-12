using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador;
using Integration.Orchestrator.Backend.Domain.Entities.Configurador.Interfaces;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Configurador;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Services.Maintainer;
using Integration.Orchestrator.Backend.Domain.Specifications;
using System.Linq.Expressions;

namespace Integration.Orchestrator.Backend.Domain.Services.Configurador
{
    [DomainService]
    public class PropertyService(
        IPropertyRepository<PropertyEntity> propertyRepository,
        ICodeConfiguratorService codeConfiguratorService,
        IStatusService<StatusEntity> statusService)
        : IPropertyService<PropertyEntity>
    {
        private readonly IPropertyRepository<PropertyEntity> _propertyRepository = propertyRepository;
        private readonly ICodeConfiguratorService _codeConfiguratorService = codeConfiguratorService;
        private readonly IStatusService<StatusEntity> _statusService = statusService;

        public async Task InsertAsync(PropertyEntity property)
        {
            await ValidateBussinesLogic(property, true);
            await _propertyRepository.InsertAsync(property);
        }

        public async Task UpdateAsync(PropertyEntity property)
        {
            await ValidateBussinesLogic(property);
            await _propertyRepository.UpdateAsync(property);
        }

        public async Task DeleteAsync(PropertyEntity property)
        {
            await _propertyRepository.DeleteAsync(property);
        }

        public async Task<PropertyEntity> GetByIdAsync(Guid id)
        {
            var specification = PropertySpecification.GetByIdExpression(id);
            return await _propertyRepository.GetByIdAsync(specification);
        }

        public async Task<PropertyEntity> GetByCodeAsync(string code)
        {
            var specification = PropertySpecification.GetByCodeExpression(code);
            return await _propertyRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<PropertyEntity>> GetByTypeIdAsync(Guid typeId)
        {
            var specification = PropertySpecification.GetByTypeExpression(typeId);
            return await _propertyRepository.GetByTypeAsync(specification);
        }

        public async Task<PropertyEntity> GetByEntityIdAsync(Guid entityId, Guid idStatusActive)
        {
            var specification = PropertySpecification.GetByEntityActiveExpression(entityId, idStatusActive);
            return await _propertyRepository.GetByEntityAsync(specification);
        }
        public async Task<IEnumerable<PropertyEntity>> GetByEntitysIdAsync(Guid entityId)
        {
            var specification = PropertySpecification.GetByEntityExpression(entityId);
            return await _propertyRepository.GetByEntitysAsync(specification);
        }

        public async Task<IEnumerable<PropertyEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            if (string.IsNullOrEmpty(paginatedModel.Sort_field))
            {
                paginatedModel.Sort_field = nameof(PropertyEntity.updated_at).Split("_")[0];
                paginatedModel.Sort_order = SortOrdering.Descending;
            }
            var spec = new PropertySpecification(paginatedModel);

            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _propertyRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new PropertySpecification(paginatedModel);
            
            if (paginatedModel.activeOnly == true)
            {
                spec.Criteria = await ActiveStatusCriteria(spec.Criteria);
            }
            return await _propertyRepository.GetTotalRows(spec);
        }

        private async Task<Expression<Func<PropertyEntity, bool>>> ActiveStatusCriteria(Expression<Func<PropertyEntity, bool>> criteria)
        {
            var entityFound = await _statusService.GetByKeyAsync(Status.active.ToString());
            return criteria = criteria.And(x => x.status_id == entityFound.id);
        }

        private async Task ValidateBussinesLogic(PropertyEntity property, bool create = false)
        {
            await EnsureStatusExists(property.status_id);
            await IsDuplicateNameAndEntity(property);

            if (create)
            {
                var codeFound = await _codeConfiguratorService.GenerateCodeAsync(Prefix.Property);
                await EnsureCodeIsUnique(codeFound);
                property.property_code = codeFound;
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

        private async Task IsDuplicateNameAndEntity(PropertyEntity property)
        {
            var validate = await _propertyRepository.ValidateNameAndEntity(property);
            if (validate)
            {
                throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = AppMessages.Domain_PropertyExists
                        });
            }
        }

    }
}
