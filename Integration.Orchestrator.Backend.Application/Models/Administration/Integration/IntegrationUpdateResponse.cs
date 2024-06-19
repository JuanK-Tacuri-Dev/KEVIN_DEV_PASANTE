namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    public class IntegrationUpdateResponse : ModelResponse<IntegrationUpdate>
    {
    }
    public class IntegrationUpdate()
    {
        public Guid Id { get; set; }
    }
}
