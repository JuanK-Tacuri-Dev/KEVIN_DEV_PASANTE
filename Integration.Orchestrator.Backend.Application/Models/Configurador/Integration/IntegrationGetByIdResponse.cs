namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Integration
{
    public class IntegrationGetByIdResponse : ModelResponse<IntegrationGetById>
    {
    }
    public class IntegrationGetById : IntegrationRequest
    {
        public Guid Id { get; set; }
    }
}
