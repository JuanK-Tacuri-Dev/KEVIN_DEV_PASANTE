using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
