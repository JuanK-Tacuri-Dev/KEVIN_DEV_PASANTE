using Integration.Orchestrator.Backend.Domain.Helper;
namespace Integration.Orchestrator.Backend.Domain.Entities.Configurador
{
    [Serializable]
    public class SynchronizationStatusEntity : Entity<Guid>
    {
        public string synchronization_status_key { get; set; }
        public string synchronization_status_text { get; set; }
        public string synchronization_status_color { get; set; }
        public string synchronization_status_background { get; set; }
        public string created_at { get; set; } = ConfigurationSystem.DateTimeDefault;
        public string updated_at { get; set; } = ConfigurationSystem.DateTimeDefault;
    }
}
