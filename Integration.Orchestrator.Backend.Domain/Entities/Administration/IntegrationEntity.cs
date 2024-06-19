namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class IntegrationEntity : Entity<Guid>
    {
        public string name { get; set; }
        public Guid status { get; set; }
        public string observations { get; set; }
        public Guid user_id { get; set; }
        public List<Guid> process { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;

    }


}
