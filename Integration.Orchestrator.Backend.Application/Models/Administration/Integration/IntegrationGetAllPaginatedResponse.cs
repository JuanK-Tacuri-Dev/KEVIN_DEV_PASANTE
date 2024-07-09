namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    public class IntegrationGetAllPaginatedResponse : ModelResponseGetAll<IntegrationGetAllRows> { }

    public class IntegrationGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<IntegrationGetAllPaginated> Rows { get; set; }
    }

    public class IntegrationGetAllPaginated : IntegrationResponse
    {
    }
}
