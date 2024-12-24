using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid? FranchiseId { get; set; }
        public string Observations { get; set; } = string.Empty;
        public IEnumerable<IntegrationResponse> Integrations { get; set; } = Enumerable.Empty<IntegrationResponse>();

        public string HourToExecute { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class IntegrationResponse
    {
        public Guid Id { get; set; }
        
    }
}
