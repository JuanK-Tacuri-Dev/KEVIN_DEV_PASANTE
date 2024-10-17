using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyGetByTypeRequest
    {
        public Guid TypeId { get; set; }
    }
}
