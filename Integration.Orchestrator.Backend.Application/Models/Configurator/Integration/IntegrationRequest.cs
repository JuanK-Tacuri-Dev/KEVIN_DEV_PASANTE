using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
        public string Observations { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public IEnumerable<ProcessRequest> Process { get; set; } = Enumerable.Empty<ProcessRequest>();
    }

    [ExcludeFromCodeCoverage]
    public class ProcessRequest
    {
        public Guid Id { get; set; }

    }
}
