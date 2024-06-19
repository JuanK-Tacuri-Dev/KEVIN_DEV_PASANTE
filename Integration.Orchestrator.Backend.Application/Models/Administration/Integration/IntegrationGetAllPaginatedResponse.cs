namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    public class IntegrationGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<IntegrationGetAllPaginated>>
    {
    }
    public class IntegrationGetAllPaginated : IntegrationRequest
    {
        public Guid Id { get; set; }
    }
}
