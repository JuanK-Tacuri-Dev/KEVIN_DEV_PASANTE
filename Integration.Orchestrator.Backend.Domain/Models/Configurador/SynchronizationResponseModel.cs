using Integration.Orchestrator.Backend.Domain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Orchestrator.Backend.Domain.Models.Configurador
{
    public class SynchronizationResponseModel
    {
        public Guid Id { get; set; }
        public Guid status_id { get; set; }
        public string synchronization_name { get; set; }
        public string synchronization_code { get; set; }
        public string synchronization_observations { get; set; }
        public string synchronization_hour_to_execute { get; set; }
        public List<Guid> integrations { get; set; }
        public Guid? user_id { get; set; }
        public Guid? franchise_id { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public List< SynchronizationStateResponseModel> SynchronizationStates { get; set; }

    }


    public class SynchronizationStateResponseModel
    {
        public Guid Id { get; set; }
        public string synchronization_status_key { get; set; }
        public string synchronization_status_text { get; set; }
        public string synchronization_status_color { get; set; }
        public string synchronization_status_background { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

    }
}
