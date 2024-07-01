namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class GetByIdSynchronizationResponse : ModelResponse<GetByIdSynchronization>
    {
    }
    public class GetByIdSynchronization : SynchronizationRequest
    {
        public Guid Id { get; set; }
    }
}
