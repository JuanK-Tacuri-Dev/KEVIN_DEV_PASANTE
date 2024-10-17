using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationGetByFranchiseIdRequest
    {
        public Guid FranchiseId { get; set; }
    }
}
