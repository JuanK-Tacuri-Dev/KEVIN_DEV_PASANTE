﻿using Integration.Orchestrator.Backend.Domain.Helper;
namespace Integration.Orchestrator.Backend.Domain.Entities.Administration
{
    [Serializable]
    public class SynchronizationStatusEntity : Entity<Guid>
    {
        public string synchronization_status_key { get; set; }
        public string synchronization_status_text { get; set; }
        public string synchronization_status_color { get; set; }
        public string synchronization_status_background { get; set; }
        public string created_at { get; set; } = DateTime.UtcNow.ToLocalTime().ToString(ConfigurationSystem.DateTimeFormat);
        public string updated_at { get; set; } = DateTime.UtcNow.ToLocalTime().ToString(ConfigurationSystem.DateTimeFormat);
    }
}
