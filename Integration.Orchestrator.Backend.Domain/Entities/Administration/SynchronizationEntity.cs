using Integration.Orchestrator.Backend.Domain.Helper;
using MongoDB.Bson;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class SynchronizationEntity : Entity<Guid>
    {
        public string synchronization_name { get; set; }
        public string synchronization_code { get; set; }
        public string synchronization_observations { get; set; }
        public string synchronization_hour_to_execute { get; set; }
        public List<Guid> integrations { get; set; }
        public Guid? user_id { get; set; }
        public Guid? franchise_id { get; set; }
        public Guid status_id { get; set; }
        public string created_at { get; private set; } = ConfigurationSystem.DateTimeDefault();
        public string updated_at { get; private set; } = ConfigurationSystem.DateTimeDefault();

    }


}
    