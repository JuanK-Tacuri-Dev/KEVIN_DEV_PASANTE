namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class ProcessEntity : Entity<Guid>
    {
        public string process_code { get; set; }
        public string process_type { get; set; }
        public Guid connection_id { get; set; }
        public List<ObjectEntity> entities { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
    }

    public class ObjectEntity
    {
        public Guid id { get; set; }
        public List<PropertiesEntity> Properties { get; set; }
        public List<FiltersEntity> filters { get; set; }
    }

    public class PropertiesEntity
    {
        public Guid key_id { get; set; }
    }

    public class FiltersEntity
    {
        public Guid key_id { get; set; }
        public Guid operator_id { get; set; }
        public Guid value_id { get; set; }
    }
}
