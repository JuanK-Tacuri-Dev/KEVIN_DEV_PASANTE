using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionGetByCodeRequest
    {
        public string Code { get; set; } = string.Empty;
    }
}
