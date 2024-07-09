namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    public class AdapterEntity : Entity<Guid>
    {
        public string name { get; set; }
        public string adapter_code { get; set; }
        public string adapter_type { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
        
    }
}
