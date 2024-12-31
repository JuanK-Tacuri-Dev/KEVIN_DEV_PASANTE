using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyDeleteResponse : ModelResponse<PropertyDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class PropertyDelete
    {
        public Guid Id { get; set; }
    }
}
