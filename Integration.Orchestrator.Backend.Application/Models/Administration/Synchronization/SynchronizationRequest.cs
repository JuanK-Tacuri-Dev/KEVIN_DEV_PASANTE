using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationRequest
    {
        public string Name { get; set; }
        public Guid? FranchiseId { get; set; }
        public List<IntegrationRequest> Integrations { get; set; }
        public Guid? UserId { get; set; }
        public string? HourToExecute { get; set; }
        public Guid StatusId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class IntegrationRequest 
    {
        public Guid Id { get; set; }
        
    }
}
