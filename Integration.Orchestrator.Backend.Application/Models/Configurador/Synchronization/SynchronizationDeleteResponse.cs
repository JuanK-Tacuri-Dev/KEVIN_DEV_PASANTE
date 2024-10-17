using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization
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
