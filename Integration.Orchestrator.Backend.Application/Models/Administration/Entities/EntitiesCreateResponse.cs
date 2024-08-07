namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class EntitiesCreateResponse : ModelResponse<EntitiesCreate>
    {
    }
    public class EntitiesCreate: EntitiesRequest
    {
        public Guid Id { get; set; }
    }
}
