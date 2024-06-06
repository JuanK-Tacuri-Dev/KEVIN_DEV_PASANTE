namespace Integration.Orchestrator.Backend.Application.Models.Administrations.SynchronizationStates
{
    public class SynchronizationStatesGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<SynchronizationStatesGetAllPaginated>>
    {
    }
    public class SynchronizationStatesGetAllPaginated : SynchronizationStatesRequest
    {
        public Guid Id { get; set; }
    }
}
