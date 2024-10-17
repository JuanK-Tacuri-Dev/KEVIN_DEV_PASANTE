using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
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
