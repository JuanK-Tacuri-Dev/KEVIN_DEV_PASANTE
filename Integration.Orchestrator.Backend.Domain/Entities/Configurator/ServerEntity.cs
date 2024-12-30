using Integration.Orchestrator.Backend.Domain.Helper;
namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator
{
    [Serializable]
    public class ServerEntity : Entity<Guid>
    {
        public string server_code { get; set; } = string.Empty;
        public string server_name { get; set; } = string.Empty;
        public Guid? type_id { get; set; }
        public string server_url { get; set; } = string.Empty;
        public Guid status_id { get; set; }
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        

    }


}
