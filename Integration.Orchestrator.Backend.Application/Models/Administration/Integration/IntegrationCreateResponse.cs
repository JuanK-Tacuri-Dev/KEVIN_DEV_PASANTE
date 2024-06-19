namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    public class IntegrationCreateResponse : ModelResponse<IntegrationCreate>
    {
    }
    public class IntegrationCreate()
    {
        public Guid Id { get; set; }
    }
}
