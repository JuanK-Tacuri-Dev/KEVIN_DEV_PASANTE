namespace Integration.Orchestrator.Backend.Application.Models.Administration.Property
{
    public class PropertyResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public Guid TypeId { get; set; }
        public Guid EntityId { get; set; }
        public Guid StatusId { get; set; }
    }
}
