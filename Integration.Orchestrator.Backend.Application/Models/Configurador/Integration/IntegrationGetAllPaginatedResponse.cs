using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationGetAllPaginatedResponse : ModelResponseGetAll<IntegrationGetAllRows> { }


    [ExcludeFromCodeCoverage]
    public class IntegrationGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<IntegrationGetAllPaginated> Rows { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class IntegrationGetAllPaginated : IntegrationResponse
    {
    }
}
