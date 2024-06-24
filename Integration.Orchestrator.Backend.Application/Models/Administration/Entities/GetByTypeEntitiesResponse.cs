namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class GetByTypeEntitiesResponse : ModelResponse<IEnumerable<GetByTypeEntities>>
    {
    }
    public class GetByTypeEntities : EntitiesRequest
    {
        public Guid Id { get; set; }
    }
}
