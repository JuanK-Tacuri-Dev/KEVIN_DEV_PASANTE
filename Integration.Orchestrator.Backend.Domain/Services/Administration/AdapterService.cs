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
    public class AdapterService(
        IAdapterRepository<AdapterEntity> adapterRepository) 
        : IAdapterService<AdapterEntity>
    {
        private readonly IAdapterRepository<AdapterEntity> _adapterRepository = adapterRepository;

        public async Task InsertAsync(AdapterEntity adapter)
        {
            await ValidateBussinesLogic(adapter, true);
            await _adapterRepository.InsertAsync(adapter);
        }

        public async Task UpdateAsync(AdapterEntity adapter)
        {
            await ValidateBussinesLogic(adapter);
            await _adapterRepository.UpdateAsync(adapter);
        }

        public async Task DeleteAsync(AdapterEntity adapter)
        {
            await _adapterRepository.DeleteAsync(adapter);
        }

        public async Task<AdapterEntity> GetByIdAsync(Guid id)
        {
            var specification = AdapterSpecification.GetByIdExpression(id);
            return await _adapterRepository.GetByIdAsync(specification);
        }

        public async Task<AdapterEntity> GetByCodeAsync(string code)
        {
            var specification = AdapterSpecification.GetByCodeExpression(code);
            return await _adapterRepository.GetByCodeAsync(specification);
        }

        public async Task<IEnumerable<AdapterEntity>> GetByTypeAsync(Guid typeId)
        {
            var specification = AdapterSpecification.GetByTypeExpression(typeId);
            return await _adapterRepository.GetByTypeAsync(specification);
        }

        public async Task<IEnumerable<AdapterEntity>> GetAllPaginatedAsync(PaginatedModel paginatedModel)
        {
            var spec = new AdapterSpecification(paginatedModel);
            return await _adapterRepository.GetAllAsync(spec);
        }

        public async Task<long> GetTotalRowsAsync(PaginatedModel paginatedModel)
        {
            var spec = new AdapterSpecification(paginatedModel);
            return await _adapterRepository.GetTotalRows(spec);
        }

        private async Task ValidateBussinesLogic(AdapterEntity adapter, bool create = false) 
        {
            if (create)
            {
                var adapterByCode = await GetByCodeAsync(adapter.adapter_code);
                if (adapterByCode != null) 
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
