namespace Integration.Orchestrator.Backend.Application.Models.Administration.Repository
{
    public class RepositoryResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Port { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public Guid ServerId { get; set; }
        public Guid AdapterId { get; set; }
    }
}
