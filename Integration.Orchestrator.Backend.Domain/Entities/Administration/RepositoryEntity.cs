namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    public class RepositoryEntity : Entity<Guid>
    {
        public string repository_code { get; set; }
        public string port { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public Guid server_id { get; set; }
        public Guid adapter_id { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
    }
}
