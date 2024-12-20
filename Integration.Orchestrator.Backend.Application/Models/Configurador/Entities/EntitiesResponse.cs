using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitiesResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid TypeId { get; set; }
        public string TypeEntityName { get; set; } = string.Empty;
        public Guid RepositoryId { get; set; }
        public string RepositoryName { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
    }
}
