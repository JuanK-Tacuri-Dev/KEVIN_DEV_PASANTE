using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.SynchronizationStatus
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
