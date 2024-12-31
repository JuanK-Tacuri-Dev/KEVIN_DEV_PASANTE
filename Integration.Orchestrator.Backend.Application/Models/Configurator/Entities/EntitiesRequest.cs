using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitiesRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public Guid RepositoryId { get; set; }
        public Guid StatusId { get; set; }
    }
}
