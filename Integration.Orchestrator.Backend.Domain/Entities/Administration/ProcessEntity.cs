using Integration.Orchestrator.Backend.Domain.Helper;
namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class ProcessEntity : Entity<Guid>
    {
        public string process_code { get; set; }
        public string process_name { get; set; }
        public string process_description { get; set; }
        public Guid process_type_id { get; set; }
        public Guid connection_id { get; set; }
        public Guid status_id { get; set; }
        public List<ObjectEntity> entities { get; set; }
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
    }

    public class ObjectEntity
    {
        public Guid id { get; set; }
        public List<PropertiesEntity> Properties { get; set; }
        public List<FiltersEntity> filters { get; set; }
    }

    public class PropertiesEntity
    {
        public Guid property_id { get; set; }
        public Guid internal_status_id { get; set; }
    }

    public class FiltersEntity
    {
        public Guid property_id { get; set; }
        public Guid operator_id { get; set; }
        public string value { get; set; }
    }
}
