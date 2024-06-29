namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class ProcessEntity : Entity<Guid>
    {
        public string process_code { get; set; }
        public string process_type { get; set; }
        public Guid connection_id { get; set; }
        public List<ObjectEntity> objects { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
        
    }

    public class ObjectEntity
    {
        public string name { get; set; }
        public List<FilterEntity> filters { get; set; }
    }

    public class FilterEntity
    {
        public string key { get; set; }
        public string value { get; set; }
    }
}
