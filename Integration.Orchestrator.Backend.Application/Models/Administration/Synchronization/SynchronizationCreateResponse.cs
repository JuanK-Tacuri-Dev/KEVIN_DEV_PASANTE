namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationCreateResponse : ModelResponse<SynchronizationCreate>
    {
    }
    public class SynchronizationCreate: SynchronizationRequest
    {
        public Guid Id { get; set; }
    }
}
