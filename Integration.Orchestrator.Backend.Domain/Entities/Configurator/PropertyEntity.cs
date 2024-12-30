using Integration.Orchestrator.Backend.Domain.Helper;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator
{
    public class PropertyEntity : Entity<Guid>
    {
        public string property_name { get; set; } = string.Empty;
        public string property_code { get; set; } = string.Empty;
        public Guid type_id { get; set; }
        public string typePropertyName { get; set; } = string.Empty;
        public Guid entity_id { get; set; }
        public string entityName { get; set; } = string.Empty;
        public Guid status_id { get; set; }
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        
    }
}
