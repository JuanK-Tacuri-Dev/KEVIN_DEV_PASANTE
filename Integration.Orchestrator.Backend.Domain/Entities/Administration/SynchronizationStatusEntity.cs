namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class SynchronizationStatusEntity : Entity<Guid>
    {
        public string key { get; set; }
        public string text { get; set; }
        public string color { get; set; }
        public string background { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
    }
}
