using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyGetByEntityRequest
    {
        public Guid EntityId { get; set; }
    }
}
