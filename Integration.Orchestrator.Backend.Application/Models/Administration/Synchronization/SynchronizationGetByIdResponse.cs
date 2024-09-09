namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationGetByIdResponse : ModelResponse<SynchronizationGetById>
    {
    }
    public class SynchronizationGetById : SynchronizationResponse
    {
        public Guid StatusId { get; set; }
    }
}
