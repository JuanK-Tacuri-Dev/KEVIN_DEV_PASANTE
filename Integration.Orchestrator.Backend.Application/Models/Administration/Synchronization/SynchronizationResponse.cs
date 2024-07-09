using Integration.Orchestrator.Backend.Application.Models.Administration.Status;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public StatusResponse Status { get; set; }
        public string Observations { get; set; }
        public Guid UserId { get; set; }
        public string HourToExecute { get; set; }
        public List<IntegrationRequest> Integrations { get; set; }
    }
    
    public class IntegrationResponse
    {
        public Guid Id { get; set; }
        
    }
}
