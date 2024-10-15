namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization
{
    public class SynchronizationGetByFranchiseIdResponse : ModelResponse<IEnumerable<SynchronizationGetByFranchiseId>>
    {
    }
    public class SynchronizationGetByFranchiseId : SynchronizationResponse
    {
        public Guid StatusId { get; set; }
    }
}
