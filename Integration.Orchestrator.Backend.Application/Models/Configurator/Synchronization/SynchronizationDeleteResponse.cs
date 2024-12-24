using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationDeleteResponse : ModelResponse<SynchronizationDelete>
    {
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationDelete
    {
        public Guid Id { get; set; }
    }
}
