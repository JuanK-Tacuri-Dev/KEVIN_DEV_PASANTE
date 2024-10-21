using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionGetAllPaginatedResponse : ModelResponseGetAll<ConnectionGetAllRows> { }

    [ExcludeFromCodeCoverage]
    public class ConnectionGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<ConnectionGetAllPaginated> Rows { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ConnectionGetAllPaginated : ConnectionResponse
    {
    }
}
