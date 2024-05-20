namespace Integration.Orchestrator.Backend.Domain.Ports
{
    public interface ILoader<T> where T : class
    {
        Task execute(IEnumerable<T> data);
    }
}
