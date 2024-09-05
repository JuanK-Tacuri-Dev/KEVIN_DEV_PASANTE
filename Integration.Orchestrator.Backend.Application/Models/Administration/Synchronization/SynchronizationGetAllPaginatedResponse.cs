using Integration.Orchestrator.Backend.Application.Models.Administration.Status;
using Integration.Orchestrator.Backend.Application.Models.Administration.SynchronizationStatus;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Synchronization
{
    public class SynchronizationGetAllPaginatedResponse : ModelResponseGetAll<SynchronizationGetAllRows>
    {
    }

    public class SynchronizationGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<SynchronizationGetAllPaginated> Rows { get; set; }
    }

    public class SynchronizationGetAllPaginated : SynchronizationResponse
    {
        public SynchronizationStatusResponse Status { get; set; }
    }
}
