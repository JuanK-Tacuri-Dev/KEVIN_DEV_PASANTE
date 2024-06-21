namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class GetByCodeEntitiesResponse : ModelResponse<GetByCodeEntities>
    {
    }
    public class GetByCodeEntities : EntitiesRequest
    {
        public Guid Id { get; set; }
    }
}
