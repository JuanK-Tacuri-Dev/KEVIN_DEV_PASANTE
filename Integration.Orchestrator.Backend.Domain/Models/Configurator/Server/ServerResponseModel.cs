﻿namespace Integration.Orchestrator.Backend.Domain.Models.Configurator
{
    public class ServerResponseModel
    {
        public Guid id { get; set; }
        public string server_code { get; set; } = string.Empty;
        public string server_name { get; set; } = string.Empty;
        public Guid? type_id { get; set; }
        public string type_name { get; set; } = string.Empty;
        public string server_url { get; set; } = string.Empty;
        public Guid status_id { get; set; }
    }
}
