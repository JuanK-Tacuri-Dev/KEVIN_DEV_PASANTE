namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Status
{
    public class StatusGetAllPaginatedResponse : ModelResponseGetAll<StatusGetAllRows>
    {
    }

    public class StatusGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<StatusGetAllPaginated> Rows { get; set; }
    }
    public class StatusGetAllPaginated : StatusResponse
    {
    }
}
