namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyCreateResponse : ModelResponse<PropertyCreate>
    {
    }
    public class PropertyCreate()
    {
        public Guid Id { get; set; }
    }
}
