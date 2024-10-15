namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Property
{
    public class PropertyGetByEntityResponse : ModelResponse<IEnumerable<PropertyGetByEntity>>
    {
    }
    public class PropertyGetByEntity : PropertyResponse
    {
        public Guid Id { get; set; }
    }
}
