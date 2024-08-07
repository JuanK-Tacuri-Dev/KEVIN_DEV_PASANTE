namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationGetByFranchiseIdResponse : ModelResponse<IEnumerable<SynchronizationGetByFranchiseId>>
    {
    }
    public class SynchronizationGetByFranchiseId : SynchronizationRequest
    {
        public Guid Id { get; set; }
    }
}
