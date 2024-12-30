namespace Integration.Orchestrator.Backend.Domain.Models.Configurator
{
    public class ConnectionResponseModel
    {
        public Guid id { get; set; }
        public string connection_code { get; set; } = string.Empty;
        public string connection_name { get; set; } = string.Empty;
        public string connection_description { get; set; } = string.Empty;
        public string serverName { get; set; } = string.Empty;
        public string serverUrl { get; set; } = string.Empty;
        public string adapterName { get; set; } = string.Empty;
        public string repositoryName { get; set; } = string.Empty;
        public Guid server_id { get; set; }
        public Guid adapter_id { get; set; }
        public Guid repository_id { get; set; }
        public Guid status_id { get; set; }
    }
}
