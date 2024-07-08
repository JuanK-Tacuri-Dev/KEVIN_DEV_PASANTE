namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStates
{
    public class SynchronizationStatesGetAllPaginatedResponse : ModelResponseGetAll<SynchronizationStatesGetAllRows> { }

    public class SynchronizationStatesGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<SynchronizationStatesGetAllPaginated> Rows { get; set; }
    }

    public class SynchronizationStatesGetAllPaginated : SynchronizationStatesResponse
    {
    }
}
