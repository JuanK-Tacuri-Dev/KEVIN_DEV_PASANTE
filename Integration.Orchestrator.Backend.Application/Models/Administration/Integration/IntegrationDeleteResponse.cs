namespace Integration.Orchestrator.Backend.Application.Models.Administration.Integration
{
    public class IntegrationDeleteResponse : ModelResponse<IntegrationDelete>
    {
    }
    public class IntegrationDelete 
    {
        public Guid Id { get; set; }
    }
}
