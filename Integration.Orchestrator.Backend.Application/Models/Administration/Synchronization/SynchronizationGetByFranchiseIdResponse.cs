namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationGetByFranchiseIdResponse : ModelResponse<IEnumerable<SynchronizationGetByFranchiseId>>
    {
    }
    public class SynchronizationGetByFranchiseId : SynchronizationResponse
    {
        public Guid StatusId { get; set; }
    }
}
