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
        public string Observations { get; set; }
        public List<IntegrationResponse> Integrations { get; set; }
        public SynchronizationStatusResponse Status { get; set; }

        public string HourToExecute { get; set; } = string.Empty;
        public Guid? UserId { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class IntegrationResponse
    {
        public Guid Id { get; set; }
    }

    public class SynchronizationStatusResponse
    {
        public Guid Id { get; set; }
        public string status_key { get; set; }
        public string status_text { get; set; }
        public string status_color { get; set; }
        public string status_background { get; set; }
        public string created { get; set; }
        public string updated { get; set; }

    }
}
