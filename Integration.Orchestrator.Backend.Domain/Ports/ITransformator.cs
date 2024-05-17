namespace Integration.Orchestrator.Backend.Domain.Ports
{
    public interface ITransformator<E, T> where E : class
    {
        Task<IEnumerable<T>> execute(IEnumerable<E> data);
    }
}
