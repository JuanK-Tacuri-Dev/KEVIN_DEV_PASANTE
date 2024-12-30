using Integration.Orchestrator.Backend.Domain.Entities.Configurator;

namespace Integration.Orchestrator.Backend.Domain.Models.Configurator
{
    public class ProcessResponseModel
    {
        public Guid id { get; set; }
        public string process_code { get; set; } = string.Empty;
        public string process_name { get; set; } = string.Empty;
        public string process_description { get; set; } = string.Empty;
        public string typeProcessName { get; set; } = string.Empty;
        public string connectionName { get; set; } = string.Empty;
        public Guid process_type_id { get; set; }
        public Guid connection_id { get; set; }
        public Guid status_id { get; set; }
        public IEnumerable<ObjectEntity> entities { get; set; } = Enumerable.Empty<ObjectEntity>();
    }
}
