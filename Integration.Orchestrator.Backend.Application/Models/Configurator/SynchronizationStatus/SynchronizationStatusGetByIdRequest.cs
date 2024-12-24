using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.SynchronizationStatus
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
