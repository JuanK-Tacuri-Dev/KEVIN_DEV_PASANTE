namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationUpdateResponse : ModelResponse<SynchronizationUpdate>
    {
    }
    public class SynchronizationUpdate: SynchronizationRequest
    {
        public Guid Id { get; set; }
    }
}
