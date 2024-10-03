namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationRequest
    {
        public string Name { get; set; }
        public Guid? FranchiseId { get; set; }
        public List<IntegrationRequest> Integrations { get; set; }
        public Guid? UserId { get; set; }
        public string? HourToExecute { get; set; }
        public Guid StatusId { get; set; }
    }
    
    public class IntegrationRequest 
    {
        public Guid Id { get; set; }
        
    }
}
