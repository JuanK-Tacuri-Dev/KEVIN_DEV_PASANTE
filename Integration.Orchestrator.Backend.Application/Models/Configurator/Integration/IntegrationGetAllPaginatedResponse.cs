using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationGetAllPaginatedResponse : ModelResponseGetAll<IntegrationGetAllRows> { }


    [ExcludeFromCodeCoverage]
    public class IntegrationGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<IntegrationGetAllPaginated> Rows { get; set; } = Enumerable.Empty<IntegrationGetAllPaginated>();
    }

    [ExcludeFromCodeCoverage]
    public class IntegrationGetAllPaginated : IntegrationResponse
    {
    }
}
