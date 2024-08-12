namespace Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus
{
    public class SynchronizationStatusGetAllPaginatedResponse : ModelResponseGetAll<SynchronizationStatusGetAllRows> { }

    public class SynchronizationStatusGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<SynchronizationStatusGetAllPaginated> Rows { get; set; }
    }

    public class SynchronizationStatusGetAllPaginated : SynchronizationStatusResponse
    {
    }
}
