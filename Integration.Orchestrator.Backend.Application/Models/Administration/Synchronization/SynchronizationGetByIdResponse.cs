namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationGetByIdResponse : ModelResponse<SynchronizationGetById>
    {
    }
    public class SynchronizationGetById : SynchronizationRequest
    {
        public Guid Id { get; set; }
    }
}
