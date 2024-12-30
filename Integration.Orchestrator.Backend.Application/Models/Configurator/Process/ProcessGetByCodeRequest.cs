using System.Diagnostics.CodeAnalysis;

namespace Integration.Orchestrator.Backend.Application.Models.Configurator.Process
{
    [ExcludeFromCodeCoverage]
    public class ProcessGetByCodeRequest
    {
        public string Code { get; set; } = string.Empty;
    }
}
