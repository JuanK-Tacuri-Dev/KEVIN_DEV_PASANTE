namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyDeleteResponse : ModelResponse<PropertyDelete>
    {
    }
    public class PropertyDelete
    {
        public Guid Id { get; set; }
    }
}
