using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.SynchronizationStatus
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
