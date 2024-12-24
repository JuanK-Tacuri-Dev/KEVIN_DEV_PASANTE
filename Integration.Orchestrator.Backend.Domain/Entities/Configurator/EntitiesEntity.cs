using Integration.Orchestrator.Backend.Domain.Helper;

namespace Integration.Orchestrator.Backend.Domain.Entities.Configurator
{
    public class EntitiesEntity : Entity<Guid>
    {
        public string entity_name { get; set; }
        public string entity_code { get; set; }
        public Guid type_id { get; set; }
        public string typeEntityName { get; set; }
        public Guid repository_id { get; set; }
        public string RepositoryName { get; set; } = string.Empty;
        public Guid status_id { get; set; }
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        
    }
}
