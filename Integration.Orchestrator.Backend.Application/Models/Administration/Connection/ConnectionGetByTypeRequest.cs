using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Administration.Connection
{
    [ExcludeFromCodeCoverage]
    public class ConnectionGetByTypeRequest
    {
        public string Type { get; set; }
    }
}
