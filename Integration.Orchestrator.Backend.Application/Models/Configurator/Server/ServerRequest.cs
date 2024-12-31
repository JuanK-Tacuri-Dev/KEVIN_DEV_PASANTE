using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerRequest
    {
        public string Name { get; set; } = string.Empty;
        public Guid? TypeServerId { get; set; }
        public string Url { get; set; } = string.Empty;
        public Guid StatusId { get; set; }

    }
}
