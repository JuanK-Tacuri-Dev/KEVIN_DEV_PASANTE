using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerRequest
    {
        public string Name { get; set; }
        public Guid? TypeServerId { get; set; }
        public string Url { get; set; }
        public Guid StatusId { get; set; }

    }
}
