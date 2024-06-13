namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class SynchronizationEntity : Entity<Guid>
    {
        public string name { get; set; }
        public Guid franchise_id { get; set; }
        public Guid status { get; set; }
        public string observations { get; set; }
        public List<Guid> integrations { get; set; }
        public Guid user_id { get; set; }
        public DateTime hour_to_execute { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;

    }


}
