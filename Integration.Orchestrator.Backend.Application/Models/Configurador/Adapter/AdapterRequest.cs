using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Adapter
{
    [ExcludeFromCodeCoverage]
    public class AdapterRequest
    {
        public string Name { get; set; }
        public Guid TypeAdapterId { get; set; }
        public string Version { get; set; }
        public Guid StatusId { get; set; }
    }
}
