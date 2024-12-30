namespace Integration.Orchestrator.Backend.Domain.Models.Configurator
{
    public class AdapterResponseModel
    {
        public Guid id { get; set; }
        public string adapter_code { get; set; } = string.Empty;
        public string adapter_name { get; set; } = string.Empty;
        public string adapter_version { get; set; } = string.Empty;
        public string typeAdapterName { get; set; } = string.Empty;
        public Guid type_id { get; set; }
        public Guid status_id { get; set; }
    }
}
