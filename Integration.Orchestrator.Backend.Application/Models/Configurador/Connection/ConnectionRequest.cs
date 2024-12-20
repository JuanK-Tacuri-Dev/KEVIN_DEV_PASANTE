using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurador.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionRequest
    {
        public Guid ServerId { get; set; }
        public Guid AdapterId { get; set; }
        public Guid RepositoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
        
        
    }
}
