using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Transformation
{
    [ExcludeFromCodeCoverage]
    public class TransformationGetAllPaginatedResponse : ModelResponseGetAll<TransformationGetAllRows> { }

    [ExcludeFromCodeCoverage]
    public class TransformationGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<TransformationGetAllPaginated> Rows { get; set; } = Enumerable.Empty<TransformationGetAllPaginated>();
    }

    [ExcludeFromCodeCoverage]
    public class TransformationGetAllPaginated : TransformationResponse
    {
    }
}
