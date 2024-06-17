namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates
{
    public class SynchronizationStatesGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<SynchronizationStatesGetAllPaginated>>
    {
    }
    public class SynchronizationStatesGetAllPaginated : SynchronizationStatesRequest
    {
        public Guid Id { get; set; }
    }
}
