namespace Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization
{
    public class SynchronizationEntity
    {
        public Guid id { get; set; }
        public Guid franchise_id { get; set; }
        public string status { get; set; }
        public string observations { get; set; }
        public Guid user_id { get; set; }
        public DateTime hour_to_execute { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;

    }
}
