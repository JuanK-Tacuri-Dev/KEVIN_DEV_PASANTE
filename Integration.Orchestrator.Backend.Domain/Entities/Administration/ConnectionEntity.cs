namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class ConnectionEntity : Entity<Guid>
    {
        public string connection_code { get; set; }
        public string connection_name { get; set; }
        public string? connection_description { get; set; }
        public Guid server_id { get; set; }
        public Guid adapter_id { get; set; }
        public Guid repository_id { get; set; }
        public Guid status_id { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;


    }


}
