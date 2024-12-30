using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Integration
{
    [ExcludeFromCodeCoverage]
    public class IntegrationResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public Guid Status { get; set; }
        public string Observations { get; set; } = string.Empty;
        public Guid UserId { get; set; }
        public IEnumerable<ProcessResponse> Process { get; set; } = Enumerable.Empty<ProcessResponse>();
    }

    [ExcludeFromCodeCoverage]
    public class ProcessResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

    }

}
