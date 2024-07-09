namespace Integration.Orchestrator.Backend.Application.Models.Administration.Process
{
    public class ProcessGetAllPaginatedResponse : ModelResponseGetAll<ProcessGetAllRows> { }

    public class ProcessGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<ProcessGetAllPaginated> Rows { get; set; }
    }
    public class ProcessGetAllPaginated : ProcessResponse
    {
    }
}
