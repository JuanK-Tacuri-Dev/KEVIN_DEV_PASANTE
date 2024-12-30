using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Orchestrator.Backend.Domain.Models.Configurator.Entity
{
    public class EntityResponseModel
    {
        public Guid id { get; set; }
        public string entity_code { get; set; } = string.Empty;
        public string entity_name { get; set; } = string.Empty;
        public Guid type_id { get; set; }
        public string typeEntityName { get; set; } = string.Empty;
        public Guid repository_id { get; set; }
        public string repository_name { get; set; } = string.Empty;
        public Guid status_id { get; set; }
    }
}
