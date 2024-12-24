using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.SynchronizationStatus
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusGetAllPaginatedResponse : ModelResponseGetAll<SynchronizationStatusGetAllRows> { }

    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<SynchronizationStatusGetAllPaginated> Rows { get; set; } = Enumerable.Empty<SynchronizationStatusGetAllPaginated>();
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusGetAllPaginated : SynchronizationStatusResponse
    {
    }
}
