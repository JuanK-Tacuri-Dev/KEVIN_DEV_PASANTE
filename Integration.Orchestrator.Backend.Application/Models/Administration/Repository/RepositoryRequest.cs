namespace Integration.Orchestrator.Backend.Application.Models.Administration.Repository
{
    public class RepositoryRequest
    {
        public string Code { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string DataBaseName { get; set; }
        public Guid AuthTypeId { get; set; }
        public Guid StatusId { get; set; }
    }
}
