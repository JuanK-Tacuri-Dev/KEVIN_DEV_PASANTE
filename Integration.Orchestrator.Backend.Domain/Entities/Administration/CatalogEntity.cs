namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class CatalogEntity : Entity<Guid>
    {
        public string code { get; set; }
        public string name { get; set; }
        public string value { get; set; }
        public Guid? father_id { get; set; }
        public string detail { get; set; }
        public Guid status_id { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;
    }
}
