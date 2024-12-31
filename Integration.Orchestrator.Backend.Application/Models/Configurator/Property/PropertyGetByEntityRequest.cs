using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyGetByEntityRequest
    {
        public Guid EntityId { get; set; }
    }
}
