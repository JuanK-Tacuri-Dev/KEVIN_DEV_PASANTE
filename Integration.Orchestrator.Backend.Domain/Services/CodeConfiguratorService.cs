using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;

namespace Integration.Orchestrator.Backend.Domain.Services
{
    [DomainService]
    public class CodeConfiguratorService(
        ICodeConfiguratorRepository<CodeConfiguratorEntity> codeConfiguratorRepository) 
        : ICodeConfiguratorService
    {
        private readonly ICodeConfiguratorRepository<CodeConfiguratorEntity> _codeConfiguratorRepository = codeConfiguratorRepository;

        public async Task<string> GenerateCodeAsync(Modules module)
        {
            CodeConfiguratorEntity entity = null;

            entity = await _codeConfiguratorRepository.GetByTypeAsync((int)module);
            entity ??= new CodeConfiguratorEntity()
                {
                    id = Guid.NewGuid(),
                    type = (int)module,
                    value_text = module.ToString().Substring(0, 1).ToUpper(),
                    value_number = 0
                };
           

            var moduleSequence = await _codeConfiguratorRepository.IncrementModuleSequenceAsync(entity);

            return $"{moduleSequence.value_text}{moduleSequence.value_number:000}";
        }
    }
}
