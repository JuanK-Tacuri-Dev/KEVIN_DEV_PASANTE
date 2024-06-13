namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class GetByFranchiseIdSynchronizationResponse : ModelResponse<IEnumerable<GetByFranchiseIdSynchronization>>
    {
    }
    public class GetByFranchiseIdSynchronization : SynchronizationRequest
    {
        public Guid Id { get; set; }
    }
}
