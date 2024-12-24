using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionGetByTypeRequest
    {
        public string Type { get; set; } = string.Empty;
    }
}
