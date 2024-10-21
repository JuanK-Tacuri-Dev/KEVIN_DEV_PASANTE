using Integration.Orchestrator.Backend.Domain.Helper;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador
{
    public class AdapterEntity : Entity<Guid>
    {
        public string adapter_code { get; set; }
        public string adapter_name { get; set; }
        public string adapter_version { get; set; }
        public Guid type_id { get; set; }
        public Guid status_id { get; set; }
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault;
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault;

    }
}