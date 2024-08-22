namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyGetByTypeResponse : ModelResponse<IEnumerable<PropertyGetByType>>
    {
    }
    public class PropertyGetByType : PropertyResponse
    {
        public Guid Id { get; set; }
    }
}
