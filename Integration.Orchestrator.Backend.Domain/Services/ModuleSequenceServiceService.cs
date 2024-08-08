using Integration.Orchestrator.Backend.Domain.Commons;
using Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence;
using Integration.Orchestrator.Backend.Domain.Exceptions;
using Integration.Orchestrator.Backend.Domain.Ports.Administration;

namespace Integration.Orchestrator.Backend.Domain.Services
{
    [DomainService]
    public class ModuleSequenceServiceService(
        IModuleSequenceRepository<ModuleSequenceEntity> moduleSequenceRepository) 
        : IModuleSequenceService
    {
        private readonly IModuleSequenceRepository<ModuleSequenceEntity> _moduleSequenceRepository = moduleSequenceRepository;

        public async Task<string> GenerateCodeAsync(string moduleName)
        {
            if (string.IsNullOrEmpty(moduleName))
                throw new OrchestratorArgumentException(string.Empty,
                    new DetailsArgumentErrors()
                    {
                        Code = (int)ResponseCode.NotFoundSuccessfully,
                        Description = ResponseMessageValues.GetResponseMessage(ResponseCode.NotFoundSuccessfully),
                        Data = moduleName
                    });

            var prefix = moduleName.Substring(0, 1).ToUpper();

            var moduleSequence = await _moduleSequenceRepository.IncrementModuleSequenceAsync(moduleName);

            return $"{prefix}{moduleSequence.last_sequence:000}";
        }
    }
}
