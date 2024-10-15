namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Property
{
    public class PropertyGetByIdResponse : ModelResponse<PropertyGetById>
    {
    }
    public class PropertyGetById : PropertyResponse
    {
        public Guid Id { get; set; }
    }
}
