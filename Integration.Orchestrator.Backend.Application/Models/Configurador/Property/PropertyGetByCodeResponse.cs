namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Property
{
    public class PropertyGetByCodeResponse : ModelResponse<PropertyGetByCode>
    {
    }
    public class PropertyGetByCode : PropertyResponse
    {
        public Guid Id { get; set; }
    }
}
