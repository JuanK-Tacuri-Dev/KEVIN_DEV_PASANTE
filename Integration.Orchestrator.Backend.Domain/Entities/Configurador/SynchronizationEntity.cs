namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador
{
    [Serializable]
    public class SynchronizationEntity : Entity<Guid>
    {
        public string synchronization_name { get; set; }
        public string synchronization_code { get; set; }
        public string synchronization_observations { get; set; }
        public DateTime synchronization_hour_to_execute { get; set; }
        public List<Guid> integrations { get; set; }
        public Guid? user_id { get; set; }
        public Guid? franchise_id { get; set; }
        public Guid status_id { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;

    }


}
