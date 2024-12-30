using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationGetByFranchiseIdRequest
    {
        public Guid FranchiseId { get; set; }
    }
}
