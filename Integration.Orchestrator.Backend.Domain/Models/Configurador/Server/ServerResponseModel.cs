namespace Integration.Orchestrator.Backend.Domain.Models.Configurador
{
    public class ServerResponseModel
    {
        public Guid id { get; set; }
        public string server_code { get; set; }
        public string server_name { get; set; }
        public Guid? type_id { get; set; }
        public string? type_name { get; set; }
        public string server_url { get; set; }
        public Guid status_id { get; set; }
    }
}
