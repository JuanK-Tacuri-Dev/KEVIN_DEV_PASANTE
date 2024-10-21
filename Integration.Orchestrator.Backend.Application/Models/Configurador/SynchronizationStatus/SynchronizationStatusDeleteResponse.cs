using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.SynchronizationStatus
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusDeleteResponse : ModelResponse<SynchronizationStatusDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusDelete
    {
        public Guid Id { get; set; }
    }
}
