namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Property
{
    public class PropertyGetByTypeResponse : ModelResponse<IEnumerable<PropertyGetByType>>
    {
    }
    public class PropertyGetByType : PropertyResponse
    {
        public Guid Id { get; set; }
    }
}
