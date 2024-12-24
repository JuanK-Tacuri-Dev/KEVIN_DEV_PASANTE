using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public Guid EntityId { get; set; }
        public Guid StatusId { get; set; }
    }
}
