﻿using System.ComponentModel.DataAnnotations;

namespace Integration.Orchestrator.Backend.Domain.Entities.Administrations.Synchronization
{
    public class SynchronizationEntity
    {
        [Key]
        public Guid id { get; set; }
        public string name { get; set; }
        public Guid franchise_id { get; set; }
        public Guid status { get; set; }
        public string observations { get; set; }
        public List<Guid> integrations { get; set; }
        public Guid user_id { get; set; }
        public DateTime hour_to_execute { get; set; }
        public DateTime created_at { get; private set; } = DateTime.UtcNow;
        public DateTime updated_at { get; private set; } = DateTime.UtcNow;

    }


}
