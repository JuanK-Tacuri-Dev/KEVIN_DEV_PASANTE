using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionResponse 
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public Guid ServerId { get; set; }
        public Guid AdapterId { get; set; }
        public Guid RepositoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
        public string serverName { get; set; } = string.Empty;
        public string AdapterName { get; set; } = string.Empty;
        public string RepositoryName { get; set; } = string.Empty;
        public string ServerUrl { get; set; } = string.Empty;
    }
}
