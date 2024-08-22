namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    public class PropertyEntity : Entity<Guid>
    {
        public string property_name { get; set; }
        public string property_code { get; set; }
        public Guid type_id { get; set; }
        public Guid entity_id { get; set; }
        public Guid status_id { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
        
    }
}
