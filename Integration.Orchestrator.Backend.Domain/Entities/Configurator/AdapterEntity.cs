using Integration.Orchestrator.Backend.Domain.Helper;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator
{
    public class AdapterEntity : Entity<Guid>
    {
        public string adapter_code { get; set; } = string.Empty;
        public string adapter_name { get; set; } = string.Empty;
        public string adapter_version { get; set; } = string.Empty;
        public Guid type_id { get; set; }
        public Guid status_id { get; set; }
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault();

    }
}