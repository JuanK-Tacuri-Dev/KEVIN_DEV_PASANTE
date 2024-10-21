using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Adapter
{
    [ExcludeFromCodeCoverage]
    public class AdapterGetAllPaginatedResponse : ModelResponseGetAll<AdapterGetAllRows> { }

    [ExcludeFromCodeCoverage]
    public class AdapterGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<AdapterGetAllPaginated> Rows { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class AdapterGetAllPaginated : AdapterResponse 
    { }
    
}
