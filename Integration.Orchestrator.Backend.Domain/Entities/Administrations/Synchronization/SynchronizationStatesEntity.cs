namespace Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization
{
    public class SynchronizationStatesEntity : Entity<Guid>
    {
        public string name { get; set; }
        public string code { get; set; }
        public string color { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
    }
}
