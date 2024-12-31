using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerGetByTypeRequest
    {
        public Guid Type { get; set; }
    }
}
