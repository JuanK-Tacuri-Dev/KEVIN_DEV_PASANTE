namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Server
{
    public class ServerGetAllPaginatedResponse : ModelResponseGetAll<ServerGetAllRows> { }

    public class ServerGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<ServerGetAllPaginated> Rows { get; set; }
    }

    public class ServerGetAllPaginated : ServerResponse
    {
    }
}
