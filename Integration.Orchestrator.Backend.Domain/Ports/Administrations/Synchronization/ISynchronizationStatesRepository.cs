namespace Integration.Orchestrator.Backend.Domain.Ports.Administrations.Synchronization
{
    public interface ISynchronizationStatesRepository<T>
    {
        Task InsertAsync(T entity);
        Task<T> GetByIdAsync(Guid id);
    }
}
