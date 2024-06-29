namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class EntitiesGetByTypeResponse : ModelResponse<IEnumerable<EntitiesGetByType>>
    {
    }
    public class EntitiesGetByType : EntitiesRequest
    {
        public Guid Id { get; set; }
    }
}
