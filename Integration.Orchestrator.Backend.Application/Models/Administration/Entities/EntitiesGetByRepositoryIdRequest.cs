using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitiesGetByRepositoryIdRequest
    {
        public Guid RepositoryId { get; set; }
    }
}
