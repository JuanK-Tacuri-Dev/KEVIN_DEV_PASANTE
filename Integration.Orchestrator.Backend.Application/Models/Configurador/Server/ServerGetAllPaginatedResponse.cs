using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerGetAllPaginatedResponse : ModelResponseGetAll<ServerGetAllRows> { }


    [ExcludeFromCodeCoverage]
    public class ServerGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<ServerGetAllPaginated> Rows { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ServerGetAllPaginated : ServerResponse
    {
    }
}
