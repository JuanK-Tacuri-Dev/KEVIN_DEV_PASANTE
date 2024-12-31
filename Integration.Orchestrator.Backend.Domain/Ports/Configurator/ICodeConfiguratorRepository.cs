namespace Integration.Orchestrator.Backend.Domain.Ports.Configurator
{
    public interface ICodeConfiguratorRepository<T>
    {
        Task<T> GetByTypeAsync(int type);
        Task<T> IncrementModuleSequenceAsync(T entity);
    }
}
