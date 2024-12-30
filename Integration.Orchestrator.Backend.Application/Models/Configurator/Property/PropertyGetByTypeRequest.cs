using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyGetByTypeRequest
    {
        public Guid TypeId { get; set; }
    }
}
