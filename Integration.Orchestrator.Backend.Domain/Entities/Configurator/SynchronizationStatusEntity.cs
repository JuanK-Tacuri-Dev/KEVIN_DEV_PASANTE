using Integration.Orchestrator.Backend.Domain.Helper;
namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator
{
    [Serializable]
    public class SynchronizationStatusEntity : Entity<Guid>
    {
        public string synchronization_status_key { get; set; } = string.Empty;
        public string synchronization_status_text { get; set; } = string.Empty;
        public string synchronization_status_color { get; set; } = string.Empty;
        public string synchronization_status_background { get; set; } = string.Empty;
        public string created_at { get; set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; set; } = ConfigurationSystem.DateTimeDefault();
    }
}
