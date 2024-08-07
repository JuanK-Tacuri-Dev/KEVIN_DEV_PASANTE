namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class EntitiesGetByCodeResponse : ModelResponse<GetByCodeEntities>
    {
    }
    public class GetByCodeEntities : EntitiesRequest
    {
        public Guid Id { get; set; }
    }
}
