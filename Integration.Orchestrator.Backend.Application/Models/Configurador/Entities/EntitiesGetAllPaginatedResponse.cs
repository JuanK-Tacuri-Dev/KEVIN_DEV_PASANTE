using Integration.Orchestrator.Backend.Application.Models.Configurador.Synchronization;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Entities
{
    public class EntitiesGetAllPaginatedResponse : ModelResponseGetAll<EntitiesGetAllRows> { }

    public class EntitiesGetAllRows
    {
        public long Total_rows { get; set; }

        public IEnumerable<EntitiesGetAllPaginated> Rows { get; set; }
    }

    public class EntitiesGetAllPaginated : EntitiesResponse
    {
    }
}
