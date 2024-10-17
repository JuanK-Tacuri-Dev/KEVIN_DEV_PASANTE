using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyGetByEntityRequest
    {
        public Guid EntityId { get; set; }
    }
}
