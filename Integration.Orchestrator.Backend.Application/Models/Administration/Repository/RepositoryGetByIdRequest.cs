using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
