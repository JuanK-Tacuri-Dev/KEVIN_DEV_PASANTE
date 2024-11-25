using Integration.Orchestrator.Backend.Domain.Entities.Configurador;

namespace Integration.Orchestrator.Backend.Domain.Models.Configurador
{
    public class ProcessResponseModel
    {
        public Guid id { get; set; }
        public string process_code { get; set; }
        public string process_name { get; set; }
        public string process_description { get; set; }
        public string typeProcessName { get; set; }
        public string connectionName { get; set; }
        public Guid process_type_id { get; set; }
        public Guid connection_id { get; set; }
        public Guid status_id { get; set; }
        public List<ObjectEntity> entities { get; set; }
    }
}
