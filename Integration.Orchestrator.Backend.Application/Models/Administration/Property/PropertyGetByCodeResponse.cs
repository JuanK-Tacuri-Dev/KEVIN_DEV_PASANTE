namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyGetByCodeResponse : ModelResponse<PropertyGetByCode>
    {
    }
    public class PropertyGetByCode : PropertyResponse
    {
        public Guid Id { get; set; }
    }
}
