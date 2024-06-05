namespace Integration.Orchestrator.Backend.Application.Models.Administrations.Synchronization
{
    public class SynchronizationGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<SynchronizationGetAllPaginated>>
    {
    }
    public class SynchronizationGetAllPaginated : SynchronizationRequest
    {
        public Guid Id { get; set; }
    }
}
