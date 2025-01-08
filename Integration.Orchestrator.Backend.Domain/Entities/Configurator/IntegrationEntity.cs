using Integration.Orchestrator.Backend.Domain.Helper;
namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator
{
    [Serializable]
    public class IntegrationEntity : Entity<Guid>
    {
        public string integration_name { get; set; } = string.Empty;
        public string integration_observations { get; set; } = string.Empty;
        public Guid user_id { get; set; }
        public Guid status_id { get; set; }
        public IEnumerable<Guid> process { get; set; } = [];
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault();

    }


}
