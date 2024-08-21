namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    public class AdapterEntity : Entity<Guid>
    {
        public string code { get; set; }
        public string name { get; set; }
        public Guid adapter_type_id { get; set; }
        public string version { get; set; }
        public Guid status_id { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
        
    }
}
