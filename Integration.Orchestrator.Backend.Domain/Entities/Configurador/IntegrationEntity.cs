namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador
{
    [Serializable]
    public class IntegrationEntity : Entity<Guid>
    {
        public string integration_name { get; set; }
        public string integration_observations { get; set; }
        public Guid user_id { get; set; }
        public Guid status_id { get; set; }
        public List<Guid> process { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;

    }


}
