namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class EntitiesGetAllPaginatedResponse : ModelResponseGetAll<IEnumerable<EntitiesGetAllPaginated>>
    {
    }
    public class EntitiesGetAllPaginated : EntitiesRequest
    {
        public Guid Id { get; set; }
    }
}
