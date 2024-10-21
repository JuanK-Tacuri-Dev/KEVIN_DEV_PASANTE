using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationGetByFranchiseIdRequest
    {
        public Guid FranchiseId { get; set; }
    }
}
