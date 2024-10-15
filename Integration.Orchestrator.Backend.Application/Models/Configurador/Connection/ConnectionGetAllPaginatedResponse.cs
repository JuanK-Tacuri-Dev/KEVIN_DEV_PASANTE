namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Connection
{
    public class ConnectionGetAllPaginatedResponse : ModelResponseGetAll<ConnectionGetAllRows> { }

    public class ConnectionGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<ConnectionGetAllPaginated> Rows { get; set; }
    }

    public class ConnectionGetAllPaginated : ConnectionResponse
    {
    }
}
