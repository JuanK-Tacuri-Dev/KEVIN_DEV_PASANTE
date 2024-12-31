using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid? FranchiseId { get; set; }
        public IEnumerable<IntegrationRequest> Integrations { get; set; } = Enumerable.Empty<IntegrationRequest>();
        public Guid? UserId { get; set; }
        public string HourToExecute { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class IntegrationRequest 
    {
        public Guid Id { get; set; }
        
    }
}
