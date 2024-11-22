namespace Integration.Orchestrator.Backend.Domain.Models.Configurador
{
    public class RepositoryResponseModel
    {
        public Guid id { get; set; }
        public string repository_code { get; set; }
        public string repository_databaseName { get; set; }
        public int? repository_port { get; set; }
        public string repository_userName { get; set; }
        public string repository_password { get; set; }
        public string authTypeName { get; set; }
        public Guid? auth_type_id { get; set; }
        public Guid status_id { get; set; }
    }
}
