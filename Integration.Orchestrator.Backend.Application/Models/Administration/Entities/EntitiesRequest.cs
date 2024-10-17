using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitiesRequest
    {
        public string Name { get; set; }
        public Guid TypeId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid StatusId { get; set; }
    }
}
