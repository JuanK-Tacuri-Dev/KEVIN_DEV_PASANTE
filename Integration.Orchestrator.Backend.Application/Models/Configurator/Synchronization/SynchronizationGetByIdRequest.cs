using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationGetByIdRequest
    {
        public Guid Id { get; set; }
    }
}
