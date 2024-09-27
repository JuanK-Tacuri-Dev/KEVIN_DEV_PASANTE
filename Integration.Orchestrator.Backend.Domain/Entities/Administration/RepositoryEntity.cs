namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    public class RepositoryEntity : Entity<Guid>
    {
        public string repository_code { get; set; }
        public string repository_databaseName { get; set; }
        public int? repository_port { get; set; }
        public string repository_user { get; set; }
        public string repository_password { get; set; }
        public Guid? auth_type_id { get; set; }
        public Guid status_id { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
    }
}