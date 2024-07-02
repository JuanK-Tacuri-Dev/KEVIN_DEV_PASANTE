namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class ServerEntity : Entity<Guid>
    {
        public string server_code { get; set; }
        public string name { get; set; }
        public string type { get; set; }
        public string url { get; set; }        
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
        

    }


}
