using Integration.Orchestrator.Backend.Domain.Helper;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator
{
    [Serializable]
    public class ConnectionEntity : Entity<Guid>
    {
        public string connection_code { get; set; } = string.Empty;
        public string connection_name { get; set; } = string.Empty;
        public string connection_description { get; set; } = string.Empty;
        public Guid server_id { get; set; }
        public Guid adapter_id { get; set; }
        public Guid repository_id { get; set; }
        public Guid status_id { get; set; }
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault();


    }


}
