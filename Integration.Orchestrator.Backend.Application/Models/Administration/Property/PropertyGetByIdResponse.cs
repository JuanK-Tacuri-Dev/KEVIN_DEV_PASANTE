namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyGetByIdResponse : ModelResponse<PropertyGetById>
    {
    }
    public class PropertyGetById : PropertyRequest
    {
        public Guid Id { get; set; }
    }
}
