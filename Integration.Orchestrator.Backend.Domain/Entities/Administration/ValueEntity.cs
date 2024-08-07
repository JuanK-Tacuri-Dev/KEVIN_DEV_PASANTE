namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    public class ValueEntity : Entity<Guid>
    {
        public string name { get; set; }
        public string value_code { get; set; }
        public string value_type { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
    }
}
