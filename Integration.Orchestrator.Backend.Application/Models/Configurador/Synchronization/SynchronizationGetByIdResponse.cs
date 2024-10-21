using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationGetByIdResponse : ModelResponse<SynchronizationGetById>
    {
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationGetById : SynchronizationResponse
    {
        public Guid StatusId { get; set; }
    }
}
