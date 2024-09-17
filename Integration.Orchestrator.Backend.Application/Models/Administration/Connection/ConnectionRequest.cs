namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
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
