using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyGetByTypeRequest
    {
        public Guid TypeId { get; set; }
    }
}
