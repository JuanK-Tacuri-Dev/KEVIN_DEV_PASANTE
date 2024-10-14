namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class SynchronizationStatusEntity : Entity<Guid>
    {
        public string synchronization_status_key { get; set; }
        public string synchronization_status_text { get; set; }
        public string synchronization_status_color { get; set; }
        public string synchronization_status_background { get; set; }
        public DateTime created_at { get; set; } = DateTime.UtcNow;
        public DateTime updated_at { get; set; } = DateTime.UtcNow;
    }
}
