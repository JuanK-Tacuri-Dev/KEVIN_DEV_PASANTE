using Integration.Orchestrator.Backend.Domain.Helper;
namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador
{
    public class RepositoryEntity : Entity<Guid>
    {
        public string repository_code { get; set; }
        public string repository_databaseName { get; set; }
        public int? repository_port { get; set; }
        public string repository_userName { get; set; }
        public string repository_password { get; set; }
        public Guid? auth_type_id { get; set; }
        public Guid status_id { get; set; }
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault;
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault;
    }
}