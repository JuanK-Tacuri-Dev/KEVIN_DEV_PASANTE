namespace Integration.Orchestrator.Backend.Domain.Models.Configurator
{
    public class RepositoryResponseModel
    {
        public Guid id { get; set; }
        public string repository_code { get; set; } = string.Empty;
        public string repository_databaseName { get; set; } = string.Empty;
        public int? repository_port { get; set; }
        public string repository_userName { get; set; } = string.Empty;
        public string repository_password { get; set; } = string.Empty;
        public string authTypeName { get; set; } = string.Empty;
        public Guid? auth_type_id { get; set; }
        public Guid status_id { get; set; }
    }
}
