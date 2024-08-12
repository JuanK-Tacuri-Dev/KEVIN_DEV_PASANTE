using Integration.Orchestrator.Backend.Domain.Commons;

namespace Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence
{
    public interface ICodeConfiguratorService
    {
        Task<string> GenerateCodeAsync(Modules module);
    }
}
