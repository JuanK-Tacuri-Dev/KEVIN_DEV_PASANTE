using Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitiesGetAllPaginatedResponse : ModelResponseGetAll<EntitiesGetAllRows> { }

    [ExcludeFromCodeCoverage]
    public class EntitiesGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<EntitiesGetAllPaginated> Rows { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class EntitiesGetAllPaginated : EntitiesResponse
    {
    }
}
