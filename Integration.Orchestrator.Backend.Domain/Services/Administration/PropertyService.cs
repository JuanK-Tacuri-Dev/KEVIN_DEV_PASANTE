using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    [DomainService]
    public class PropertyService(
        IPropertyRepository<PropertyEntity> propertyRepository) 
        : IPropertyService<PropertyEntity>
    {
        private readonly IPropertyRepository<PropertyEntity> _propertyRepository = propertyRepository;

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

        public async Task<IEnumerable<PropertyEntity>> GetByTypeAsync(string type)
        {
            var specification = PropertySpecification.GetByTypeExpression(type);
            return await _propertyRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<PropertyEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new PropertySpecification(paginatedModel);
            return await _propertyRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new PropertySpecification(paginatedModel);
            return await _propertyRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(PropertyEntity property, bool create = false) 
        {
            if (create)
            {
                var propertyByCode = await GetByCodeAsync(property.property_code);
                if (propertyByCode != null) 
                {
                    throw new ArgumentException(AppMessages.Domain_PropertyExists);
                }
            }
        }
    }
}
