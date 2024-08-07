namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class EntitiesUpdateResponse : ModelResponse<EntitiesUpdate>
    {
    }
    public class EntitiesUpdate : EntitiesRequest
    {
        public Guid Id { get; set; }
    }
}
