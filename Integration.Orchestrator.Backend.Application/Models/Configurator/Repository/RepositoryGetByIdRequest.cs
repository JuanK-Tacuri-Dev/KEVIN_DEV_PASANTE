using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
