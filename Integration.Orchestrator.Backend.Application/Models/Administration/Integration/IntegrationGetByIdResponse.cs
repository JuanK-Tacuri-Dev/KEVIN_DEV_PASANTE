namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    public class IntegrationGetByIdResponse : ModelResponse<IntegrationGetById>
    {
    }
    public class IntegrationGetById : IntegrationRequest
    {
        public Guid Id { get; set; }
    }
}
