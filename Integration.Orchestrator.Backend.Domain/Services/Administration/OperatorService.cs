using Integration.Orchestrator.Backend.Domain.Entities.Administration;
using Integration.Orchestrator.Backend.Domain.Entities.Administration.Interfaces;
using Integration.Orchestrator.Backend.Domain.Models;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Administration
{
    public class OperatorService(
        IOperatorRepository<OperatorEntity> operatorRepository) 
        : IOperatorService<OperatorEntity>
    {
        private readonly IOperatorRepository<OperatorEntity> _operatorRepository = operatorRepository;

        public async Task InsertAsync(OperatorEntity operatorEntity)
        {
            await ValidateBussinesLogic(operatorEntity, true);
            await _operatorRepository.InsertAsync(operatorEntity);
        }

        public async Task UpdateAsync(OperatorEntity operatorEntity)
        {
            await ValidateBussinesLogic(operatorEntity);
            await _operatorRepository.UpdateAsync(operatorEntity);
        }

        public async Task DeleteAsync(OperatorEntity operatorEntity)
        {
            await _operatorRepository.DeleteAsync(operatorEntity);
        }

        public async Task<OperatorEntity> GetByIdAsync(Guid id)
        {
            var specification = OperatorSpecification.GetByIdExpression(id);
            return await _operatorRepository.GetByIdAsync(specification);
        }

        public async Task<OperatorEntity> GetByCodeAsync(string code)
        {
            var specification = OperatorSpecification.GetByCodeExpression(code);
            return await _operatorRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<OperatorEntity>> GetByTypeAsync(string type)
        {
            var specification = OperatorSpecification.GetByTypeExpression(type);
            return await _operatorRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<OperatorEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new OperatorSpecification(paginatedModel);
            return await _operatorRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new OperatorSpecification(paginatedModel);
            return await _operatorRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(OperatorEntity operatorEntity, bool create = false) 
        {
            if (create)
            {
                var operatorByCode = await GetByCodeAsync(operatorEntity.operator_code);
                if (operatorByCode != null) 
                {
                    throw new ArgumentException(AppMessages.Domain_OperatorExists);
                }
            }
        }
    }
}
