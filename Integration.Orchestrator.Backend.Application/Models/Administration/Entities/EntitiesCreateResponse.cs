namespace Integration.Orchestrator.Backend.Application.Models.Administration.Entities
{
    public class EntitiesCreateResponse : ModelResponse<EntitiesCreate>
    {
    }
    public class EntitiesCreate()
    {
        public Guid Id { get; set; }
    }
}
