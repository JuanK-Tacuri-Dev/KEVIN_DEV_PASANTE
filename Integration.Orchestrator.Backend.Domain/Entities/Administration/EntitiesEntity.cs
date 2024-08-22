namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    public class EntitiesEntity : Entity<Guid>
    {
        public string entity_name { get; set; }
        public string entity_code { get; set; }
        public Guid type_id { get; set; }
        public Guid repository_id { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
        
    }
}
