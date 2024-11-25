namespace Integration.Orchestrator.Backend.Domain.Models.Configurador
{
    public class ConnectionResponseModel
    {
        public Guid id { get; set; }
        public string connection_code { get; set; }
        public string connection_name { get; set; }
        public string? connection_description { get; set; }
        public string serverName { get; set; }
        public string adapterName { get; set; }
        public string repositoryName { get; set; }
        public Guid server_id { get; set; }
        public Guid adapter_id { get; set; }
        public Guid repository_id { get; set; }
        public Guid status_id { get; set; }
    }
}
