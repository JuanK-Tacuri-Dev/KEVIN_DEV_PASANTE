namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Connection
{
    public class ConnectionRequest
    {
        public Guid ServerId { get; set; }
        public Guid AdapterId { get; set; }
        public Guid RepositoryId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid StatusId { get; set; }
        
        
    }
}
