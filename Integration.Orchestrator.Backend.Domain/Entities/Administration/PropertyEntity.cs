namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    public class PropertyEntity : Entity<Guid>
    {
        public string name { get; set; }
        public string property_code { get; set; }
        public string property_type { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
    }
}
