namespace Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization
{
    public class SynchronizationCreateResponse : ModelResponse<SynchronizationCreate>
    {
    }
    public class SynchronizationCreate()
    {
        public Guid Id { get; set; }
    }
}
