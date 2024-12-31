using Integration.Orchestrator.Backend.Domain.Helper;
namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator
{
    [Serializable]
    public class ProcessEntity : Entity<Guid>
    {
        public string process_code { get; set; } = string.Empty;
        public string process_name { get; set; } = string.Empty;
        public string process_description { get; set; } = string.Empty;
        public Guid process_type_id { get; set; }
        public Guid connection_id { get; set; }
        public Guid status_id { get; set; }
        public IEnumerable<ObjectEntity> entities { get; set; } = Enumerable.Empty<ObjectEntity>();
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
    }

    public class ObjectEntity
    {
        public Guid id { get; set; }
        public IEnumerable<PropertiesEntity> Properties { get; set; } = Enumerable.Empty<PropertiesEntity>();
        public IEnumerable<FiltersEntity> filters { get; set; } = Enumerable.Empty<FiltersEntity>();
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
        public string value { get; set; } = string.Empty;
    }
}
