namespace Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization
{
    public class SynchronizationUpdateResponse : ModelResponse<SynchronizationUpdate>
    {
    }
    public class SynchronizationUpdate()
    {
        public Guid Id { get; set; }
    }
}
