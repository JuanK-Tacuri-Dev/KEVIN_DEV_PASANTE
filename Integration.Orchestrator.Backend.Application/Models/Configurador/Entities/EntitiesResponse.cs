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
        public Guid RepositoryId { get; set; }
        public Guid StatusId { get; set; }
    }
}
