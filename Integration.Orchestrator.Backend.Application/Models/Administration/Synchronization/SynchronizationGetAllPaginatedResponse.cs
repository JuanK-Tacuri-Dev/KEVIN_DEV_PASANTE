namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<SynchronizationGetAllPaginated>>
    {
    }
    public class SynchronizationGetAllPaginated : SynchronizationRequest
    {
        public Guid Id { get; set; }
    }
}
