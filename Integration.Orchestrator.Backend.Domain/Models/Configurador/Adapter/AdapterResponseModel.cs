namespace Integration.Orchestrator.Backend.Domain.Models.Configurador
{
    public class AdapterResponseModel
    {
        public Guid id { get; set; }
        public string adapter_code { get; set; }
        public string adapter_name { get; set; }
        public string adapter_version { get; set; }
        public string typeAdapterName { get; set; }
        public Guid type_id { get; set; }
        public Guid status_id { get; set; }
    }
}
