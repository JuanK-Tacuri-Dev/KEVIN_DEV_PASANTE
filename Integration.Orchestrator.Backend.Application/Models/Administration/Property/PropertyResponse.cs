namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Type { get; set; }
        public Guid EntityId { get; set; }
    }
}
