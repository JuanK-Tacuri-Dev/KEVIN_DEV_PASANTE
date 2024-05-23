namespace Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization
{
    public class GetByFranchiseIdSynchronizationResponse : ModelResponse<IEnumerable<GetByFranchiseIdSynchronization>>
    {
    }
    public class GetByFranchiseIdSynchronization : SynchronizationRequest
    {
        public Guid Id { get; set; }
    }
}
