using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Repository
{
    [ExcludeFromCodeCoverage]
    public class RepositoryDeleteResponse : ModelResponse<RepositoryDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class RepositoryDelete
    {
        public Guid Id { get; set; }
    }
}
