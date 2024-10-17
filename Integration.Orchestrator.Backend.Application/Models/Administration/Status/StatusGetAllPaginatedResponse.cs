using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Status
{
    [ExcludeFromCodeCoverage]
    public class StatusGetAllPaginatedResponse : ModelResponseGetAll<StatusGetAllRows>
    {
    }

    [ExcludeFromCodeCoverage]
    public class StatusGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<StatusGetAllPaginated> Rows { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class StatusGetAllPaginated : StatusResponse
    {
    }
}
