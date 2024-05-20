namespace Integration.Orchestrator.Backend.Domain.Ports
{
    public interface IExtractor<E> where E : class
    {
        Task<IEnumerable<E>> execute();
    }
}
