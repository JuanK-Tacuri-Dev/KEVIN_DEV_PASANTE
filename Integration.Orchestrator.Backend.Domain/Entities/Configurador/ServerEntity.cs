namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador
{
    [Serializable]
    public class ServerEntity : Entity<Guid>
    {
        public string server_code { get; set; }
        public string server_name { get; set; }
        public Guid? type_id { get; set; }
        public string server_url { get; set; }        
        public Guid status_id { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
        

    }


}
