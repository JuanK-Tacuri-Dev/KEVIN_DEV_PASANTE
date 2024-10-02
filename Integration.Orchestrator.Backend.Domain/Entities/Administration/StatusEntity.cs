namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    public class StatusEntity : Entity<Guid>
    {
        public string status_key { get; set; }
        public string status_text { get; set; }
        public string status_color { get; set; }
        public string status_background { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
    }
}