namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyRequest
    {
        public string Name { get; set; }
        public Guid TypeId { get; set; }
        public Guid EntityId { get; set; }
        public Guid StatusId { get; set; }
    }
}
