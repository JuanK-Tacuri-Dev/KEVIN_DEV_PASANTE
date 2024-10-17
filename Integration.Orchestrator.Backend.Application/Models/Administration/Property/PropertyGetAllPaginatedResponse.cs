using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    [ExcludeFromCodeCoverage]
    public class PropertyGetAllPaginatedResponse : ModelResponseGetAll<PropertyGetAllRows> { }

    [ExcludeFromCodeCoverage]
    public class PropertyGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<PropertyGetAllPaginated> Rows { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class PropertyGetAllPaginated : PropertyResponse
    {
    }
}
