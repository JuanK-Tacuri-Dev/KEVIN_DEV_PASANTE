using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitiesGetByTypeRequest
    {
        public Guid TypeId { get; set; }
    }
}
