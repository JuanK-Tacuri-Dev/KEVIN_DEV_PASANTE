using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitiesResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public Guid TypeId { get; set; }
        public string TypeEntityName { get; set; } = string.Empty;
        public Guid RepositoryId { get; set; }
        public string RepositoryName { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
    }
}
