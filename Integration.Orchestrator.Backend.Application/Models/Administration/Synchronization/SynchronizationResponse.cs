namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public Guid? FranchiseId { get; set; }
        public string Observations { get; set; }
        public List<IntegrationResponse> Integrations { get; set; }

        public string? HourToExecute { get; set; }
        public Guid UserId { get; set; }
    }
    
    public class IntegrationResponse
    {
        public Guid Id { get; set; }
        
    }
}
