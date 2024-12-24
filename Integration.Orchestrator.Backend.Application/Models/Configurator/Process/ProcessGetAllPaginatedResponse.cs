using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Process
{
    [ExcludeFromCodeCoverage]
    public class ProcessGetAllPaginatedResponse : ModelResponseGetAll<ProcessGetAllRows> { }

    [ExcludeFromCodeCoverage]
    public class ProcessGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<ProcessGetAllPaginated> Rows { get; set; } = Enumerable.Empty<ProcessGetAllPaginated>();
    }

    [ExcludeFromCodeCoverage]
    public class ProcessGetAllPaginated : ProcessResponse
    {
    }
}
