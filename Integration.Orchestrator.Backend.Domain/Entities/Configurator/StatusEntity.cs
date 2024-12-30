using Integration.Orchestrator.Backend.Domain.Helper;
namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator
{
    public class StatusEntity : Entity<Guid>
    {
        public string status_key { get; set; } = string.Empty;
        public string status_text { get; set; } = string.Empty;
        public string status_color { get; set; } = string.Empty;
        public string status_background { get; set; } = string.Empty;
        public string created_at { get; set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; set; } = ConfigurationSystem.DateTimeDefault();
    }
}