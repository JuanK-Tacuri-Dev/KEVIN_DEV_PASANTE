using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Adapter
{
    [ExcludeFromCodeCoverage]
    public class AdapterGetByTypeRequest
    {
        public Guid TypeAdapterId { get; set; }
    }
}
