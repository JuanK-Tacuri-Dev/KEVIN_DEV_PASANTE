namespace Integration.Orchestrator.Backend.Domain.Ports.Administration
{
    public interface IModuleSequenceRepository<T>
    {
        Task<T> GetByModuleNameAsync(string moduleName);
        Task<T> UpdateModuleSequenceAsync(string moduleName, int newSequence);
        Task<T> IncrementModuleSequenceAsync(string moduleName);
    }
}
