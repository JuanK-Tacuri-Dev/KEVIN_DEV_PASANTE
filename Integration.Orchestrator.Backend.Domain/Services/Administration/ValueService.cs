using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    [DomainService]
    public class ValueService(
        IValueRepository<ValueEntity> valueRepository) 
        : IValueService<ValueEntity>
    {
        private readonly IValueRepository<ValueEntity> _valueRepository = valueRepository;

        public async Task InsertAsync(ValueEntity value)
        {
            await ValidateBussinesLogic(value, true);
            await _valueRepository.InsertAsync(value);
        }

        public async Task UpdateAsync(ValueEntity value)
        {
            await ValidateBussinesLogic(value);
            await _valueRepository.UpdateAsync(value);
        }

        public async Task DeleteAsync(ValueEntity value)
        {
            await _valueRepository.DeleteAsync(value);
        }

        public async Task<ValueEntity> GetByIdAsync(Guid id)
        {
            var specification = ValueSpecification.GetByIdExpression(id);
            return await _valueRepository.GetByIdAsync(specification);
        }

        public async Task<ValueEntity> GetByCodeAsync(string code)
        {
            var specification = ValueSpecification.GetByCodeExpression(code);
            return await _valueRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<ValueEntity>> GetByTypeAsync(Guid typeId)
        {
            var specification = ValueSpecification.GetByTypeExpression(typeId);
            return await _valueRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<ValueEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new ValueSpecification(paginatedModel);
            return await _valueRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new ValueSpecification(paginatedModel);
            return await _valueRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(ValueEntity value, bool create = false) 
        {
            if (create)
            {
                var valueByCode = await GetByCodeAsync(value.value_code);
                if (valueByCode != null) 
                {
                    throw new ArgumentException(AppMessages.Domain_ValueExists);
                }
            }
        }
    }
}
