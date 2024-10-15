namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Integration
{
    public class IntegrationDeleteResponse : ModelResponse<IntegrationDelete>
    {
    }
    public class IntegrationDelete 
    {
        public Guid Id { get; set; }
    }
}
