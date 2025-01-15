using Integration.Orchestrator.Backend.Domain.Helper;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator
{
    [Serializable]
    public class TransformationEntity : Entity<Guid>
    {
        public string transformation_code { get; set; } = string.Empty;
        public string transformation_name { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
    }
}
