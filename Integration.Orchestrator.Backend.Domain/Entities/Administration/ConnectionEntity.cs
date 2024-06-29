namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class ConnectionEntity : Entity<Guid>
    {
        public string connection_code { get; set; }
        public string server { get; set; }
        public string port { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string adapter { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
        

    }


}
