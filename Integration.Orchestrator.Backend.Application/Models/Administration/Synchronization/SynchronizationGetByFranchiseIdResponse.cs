using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationGetByFranchiseIdResponse : ModelResponse<IEnumerable<SynchronizationGetByFranchiseId>>
    {
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationGetByFranchiseId : SynchronizationResponse
    {
        public Guid StatusId { get; set; }
    }
}
