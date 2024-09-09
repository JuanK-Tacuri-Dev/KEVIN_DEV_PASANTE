namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyGetByEntityResponse : ModelResponse<IEnumerable<PropertyGetByEntity>>
    {
    }
    public class PropertyGetByEntity : PropertyResponse
    {
        public Guid Id { get; set; }
    }
}
