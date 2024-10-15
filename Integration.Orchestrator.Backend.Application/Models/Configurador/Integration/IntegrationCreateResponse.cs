namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Integration
{
    public class IntegrationCreateResponse : ModelResponse<IntegrationCreate>
    {
    }
    public class IntegrationCreate : IntegrationRequest
    {
        public Guid Id { get; set; }
    }
}
