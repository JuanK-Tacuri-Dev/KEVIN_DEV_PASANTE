using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.SynchronizationStatus
{
    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusGetAllPaginatedResponse : ModelResponseGetAll<SynchronizationStatusGetAllRows> { }

    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<SynchronizationStatusGetAllPaginated> Rows { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class SynchronizationStatusGetAllPaginated : SynchronizationStatusResponse
    {
    }
}
