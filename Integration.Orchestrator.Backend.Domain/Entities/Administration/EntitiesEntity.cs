namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    public class EntitiesEntity : Entity<Guid>
    {
        public string name { get; set; }
        public string entity_code { get; set; }
        public string entity_type { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
    }
}
