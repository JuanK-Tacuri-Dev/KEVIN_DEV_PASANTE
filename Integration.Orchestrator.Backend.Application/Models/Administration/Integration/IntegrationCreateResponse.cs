namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    public class IntegrationCreateResponse : ModelResponse<IntegrationCreate>
    {
    }
    public class IntegrationCreate : IntegrationRequest
    {
        public Guid Id { get; set; }
    }
}
