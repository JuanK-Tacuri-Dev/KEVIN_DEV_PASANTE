namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Integration
{
    public class IntegrationUpdateResponse : ModelResponse<IntegrationUpdate>
    {
    }
    public class IntegrationUpdate : IntegrationRequest
    {
        public Guid Id { get; set; }
    }
}
