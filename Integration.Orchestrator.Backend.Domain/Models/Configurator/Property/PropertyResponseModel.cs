using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Orchestrator.Backend.Domain.Models.Configurator.Property
{
    public class PropertyResponseModel
    {
        public Guid id { get; set; }
        public string property_name { get; set; }
        public string property_code { get; set; }
        public Guid type_id { get; set; }
        public string typePropertyName { get; set; }
        public Guid entity_id { get; set; }
        public string entityName { get; set; }
        public Guid statusId { get; set; }
    }


}
