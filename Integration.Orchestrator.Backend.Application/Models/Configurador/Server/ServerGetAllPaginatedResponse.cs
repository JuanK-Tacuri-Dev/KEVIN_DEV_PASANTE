using Integration.Orchestrator.Backend.Domain.Models.Configurador.Server;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerGetAllPaginatedResponse : ModelResponseGetAll<ServerGetAllRows>
    {
        public ServerGetAllPaginatedResponse(int Code, string Description, long Totalrows, IEnumerable<ServerResponseTest> Rows = null)
        {
            this.Code = Code;
            this.Description = Description;
            this.Data = new ServerGetAllRows(Totalrows, Rows);
        }

    }


    [ExcludeFromCodeCoverage]
    public class ServerGetAllRows
    {
        public ServerGetAllRows(long TotalRows, IEnumerable<ServerResponseTest> Rows = null)
        {
            this.Total_rows = TotalRows;
            this.Rows = Rows ?? [];
        }

        public long Total_rows { get; set; }
        public IEnumerable<ServerResponseTest> Rows { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ServerGetAllPaginated : ServerResponseTest
    {
    }
}
