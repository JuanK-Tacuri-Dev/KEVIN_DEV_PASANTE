namespace Integration.Orchestrator.Backend.Domain.Entities.ModuleSequence
{
    public interface IModuleSequenceService
    {
        Task<string> GenerateCodeAsync(string moduleName);
    }
}
