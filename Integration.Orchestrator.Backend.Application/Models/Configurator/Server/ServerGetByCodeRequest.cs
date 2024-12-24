using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Server
{
    [ExcludeFromCodeCoverage]
    public class ServerGetByCodeRequest
    {
        public string Code { get; set; } = string.Empty;
    }
}
