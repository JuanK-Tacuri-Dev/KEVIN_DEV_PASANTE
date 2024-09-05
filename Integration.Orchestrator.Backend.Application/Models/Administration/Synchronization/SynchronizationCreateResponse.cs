namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationCreateResponse : ModelResponse<SynchronizationCreate>
    {
    }
    public class SynchronizationCreate: SynchronizationResponse
    {
        public Guid StatusId { get; set; }
    }
}
