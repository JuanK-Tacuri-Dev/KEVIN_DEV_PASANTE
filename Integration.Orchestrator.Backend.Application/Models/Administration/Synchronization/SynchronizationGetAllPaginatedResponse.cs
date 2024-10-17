using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationGetAllPaginatedResponse : ModelResponseGetAll<SynchronizationGetAllRows>
    {
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<SynchronizationGetAllPaginated> Rows { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationGetAllPaginated : SynchronizationResponse
    {
        public SynchronizationStatusResponse Status { get; set; }
    }
}
