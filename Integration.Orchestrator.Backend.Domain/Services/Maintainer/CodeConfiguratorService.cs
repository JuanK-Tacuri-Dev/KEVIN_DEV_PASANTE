using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.Configurator;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Ports.Configurator;
using Integration.Orchestrator.Backend.Domain.Resources;
using Integration.Orchestrator.Backend.Domain.Specifications;

namespace Integration.Orchestrator.Backend.Domain.Services.Maintainer
{
    [DomainService]
    public class CodeConfiguratorService(
        ICodeConfiguratorRepository<CodeConfiguratorEntity> codeConfiguratorRepository,
        ICatalogRepository<CatalogEntity> catalogRepository)
        : ICodeConfiguratorService
    {
        private readonly ICodeConfiguratorRepository<CodeConfiguratorEntity> _codeConfiguratorRepository = codeConfiguratorRepository;
        private readonly ICatalogRepository<CatalogEntity> _catalogRepository = catalogRepository;

        public async Task<string> GenerateCodeAsync(Prefix prefix)
        {
            var specification = CatalogSpecification.GetByCodeExpression((int)prefix);
            var catalog = await _catalogRepository.GetByCodeAsync(specification);
            if (catalog == null)
            {
                throw new OrchestratorArgumentException(string.Empty,
                        new DetailsArgumentErrors()
                        {
                            Code = (int)ResponseCode.NotFoundSuccessfully,
                            Description = string.Format(AppMessages.Domain_NonParameterizedPrefix,prefix.ToString())                        });
            }
            CodeConfiguratorEntity entity = null;

            entity = await _codeConfiguratorRepository.GetByTypeAsync((int)prefix);
            entity ??= new CodeConfiguratorEntity()
            {
                id = Guid.NewGuid(),
                type = (int)prefix,
                value_text = catalog.catalog_value.ToUpper(),
                value_number = 0
            };


            var moduleSequence = await _codeConfiguratorRepository.IncrementModuleSequenceAsync(entity);

            return $"{moduleSequence.value_text}{moduleSequence.value_number:000}";
        }
    }
}
