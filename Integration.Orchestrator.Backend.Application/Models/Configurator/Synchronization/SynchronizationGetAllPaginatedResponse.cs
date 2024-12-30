using Integration.Orchestrator.Backend.Application.Models.Configurator.SynchronizationStatus;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Synchronization
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationGetAllPaginatedResponse : ModelResponseGetAll<SynchronizationGetAllRows>
    {
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<SynchronizationGetAllPaginated> Rows { get; set; } = Enumerable.Empty<SynchronizationGetAllPaginated>();
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationGetAllPaginated : SynchronizationResponse
    {
        public SynchronizationStatusResponse Status { get; set; } = new SynchronizationStatusResponse();
    }
}
