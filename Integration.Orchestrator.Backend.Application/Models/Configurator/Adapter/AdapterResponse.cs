using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Adapter
{
    [ExcludeFromCodeCoverage]
    public class AdapterResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid TypeAdapterId { get; set; }
        public string Version { get; set; } = string.Empty;
        public string TypeAdapterName { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
    }
}
