using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Adapter
{
    [ExcludeFromCodeCoverage]
    public class AdapterRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid TypeAdapterId { get; set; }
        public string Version { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
    }
}
