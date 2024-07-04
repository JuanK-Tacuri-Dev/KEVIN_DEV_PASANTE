namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class EntitiesGetByIdResponse : ModelResponse<EntitiesGetById>
    {
    }
    public class EntitiesGetById : EntitiesRequest
    {
        public Guid Id { get; set; }
    }
}
