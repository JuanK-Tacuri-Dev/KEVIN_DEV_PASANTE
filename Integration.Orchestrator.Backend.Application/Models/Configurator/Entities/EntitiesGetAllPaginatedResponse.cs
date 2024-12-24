using Integration.Orchestrator.Backend.Application.Models.Configurator.Synchronization;
using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Entities
{
    [ExcludeFromCodeCoverage]
    public class EntitiesGetAllPaginatedResponse : ModelResponseGetAll<EntitiesGetAllRows> { }

    [ExcludeFromCodeCoverage]
    public class EntitiesGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<EntitiesGetAllPaginated> Rows { get; set; } = Enumerable.Empty<EntitiesGetAllPaginated>();
    }

    [ExcludeFromCodeCoverage]
    public class EntitiesGetAllPaginated : EntitiesResponse
    {
    }
}
