using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerResponse
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public Guid? TypeServerId { get; set; }
        public string TypeServerName { get; set; } = string.Empty;
        public string Url { get; set; } = string.Empty;
        public Guid StatusId { get; set; }

    }
}
